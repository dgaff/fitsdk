////////////////////////////////////////////////////////////////////////////////
// The following FIT Protocol software provided may be used with FIT protocol
// devices only and remains the copyrighted property of Dynastream Innovations Inc.
// The software is being provided on an "as-is" basis and as an accommodation,
// and therefore all warranties, representations, or guarantees of any kind
// (whether express, implied or statutory) including, without limitation,
// warranties of merchantability, non-infringement, or fitness for a particular
// purpose, are specifically disclaimed.
//
// Copyright 2015 Dynastream Innovations Inc.
////////////////////////////////////////////////////////////////////////////////
// ****WARNING****  This file is auto-generated!  Do NOT edit this file.
// Profile Version = 16.00Release
// Tag = development-akw-16.00.00-0
////////////////////////////////////////////////////////////////////////////////



package com.garmin.fit;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.HashMap;

public class MonitoringReader implements MonitoringInfoMesgListener,
      MonitoringMesgListener, DeviceSettingsMesgListener {
   public static final int DAILY_INTERVAL = 86400;

   private class ExtractState {
      public float cyclesToCaloriesStartCycles = 0;
      public int cyclesToCaloriesStartCal = 0;
      public float cyclesToDistanceStartCycles = 0;
      public float cyclesToDistanceStartDist = 0;
   }

   private final String[] accumulatedFieldNames = {
         "cycles",
         "distance",
         "active_calories",
         "calories",
         "active_time"
   };
   
   private final String[] instantaneousFieldNames = {
         "intensity",
         "heart_rate",
         "temperature"
   };
   
   private ArrayList<MonitoringMesgListener> listeners;
   private int interval;
   private boolean outputDailyTotals;
   private MonitoringInfoMesg infoMesg;
   private HashMap<ActivityType, ArrayList<MonitoringMesg>> intervalMesgs;
   private HashMap<ActivityType, MonitoringMesg> lastMesgs;
   private long startTimestamp;
   private long endTimestamp;
   private long lastTimestamp;
   private long localTimeOffset;
   private long mesgTimestamp; // Last message timestamp, not converted from
                               // system time. Used to extract compressed
                               // timestamp fields.
   private long systemToUtcTimestampOffset;
   private long systemToLocalTimestampOffset;
   private HashMap<ActivityType, ExtractState> extractStates;

   /**
    * @param interval
    *           Duration of time to be contained in the broadcast monitoring
    *           messages.
    */
   public MonitoringReader(int interval) {
      if ((interval < 0) || (interval > DAILY_INTERVAL))
         throw new FitRuntimeException(
               interval
                     + "s is invalid.  Output interval duration must be between 1s and 86400s (1 day).");

      listeners = new ArrayList<MonitoringMesgListener>();
      this.interval = interval;
      outputDailyTotals = false;
      intervalMesgs = new HashMap<ActivityType, ArrayList<MonitoringMesg>>();
      lastMesgs = new HashMap<ActivityType, MonitoringMesg>();
      localTimeOffset = 0;
      systemToUtcTimestampOffset = 0;
      systemToLocalTimestampOffset = 0;
      extractStates = new HashMap<ActivityType, ExtractState>();
   }

   /**
    * Enables output of data from start of day instead of start of file.
    * Cumulative fields such as steps are accumulated from the start of the day
    * so daily totals can be computed from a file that does not include data for
    * earlier in the day.
    * 
    * Instantaneous fields such as intensity and heart rate are not output
    * in this mode because data for the whole day is required.
    */
   public void outputDailyTotals() {
      if (interval != DAILY_INTERVAL) {
         throw new FitRuntimeException("Interval must be 86400s to output daily totals");
      }

      outputDailyTotals = true;
   }

   /**
    * Set offset in seconds from system time to UTC time. Used to convert system
    * timestamps to UTC.
    * 
    * @param offset
    *           UTC offset in seconds
    */
   public void setSystemToUtcTimestampOffset(long offset) {
      systemToUtcTimestampOffset = offset;
   }

   /**
    * Set offset in seconds from system time to local time. Used to convert
    * system timestamps to local time.
    * 
    * @param offset
    *           local time offset in seconds
    */
   public void setSystemToLocalTimestampOffset(long offset) {
      systemToLocalTimestampOffset = offset;
   }

   /**
    * Adds a listener for decoded monitoring data. Listener will receive
    * monitoring data at interval specified in constructor. Data in the
    * monitoring message is the total for the interval (not cumulative).
    * 
    * @param mesgListener
    *           Listener for output monitoring data messages
    */
   public void addListener(MonitoringMesgListener mesgListener) {
      listeners.add(mesgListener);
   }

   /**
    * Broadcast all pending monitoring data. Call after reading file to flush
    * partial intervals. If pending monitoring data does not align to interval
    * boundary then timestamp will correspond to end of data and duration
    * indicates partial interval.
    */
   public void broadcast() {
      while (broadcastInterval(false));
   }
   
   private void broadcastCompleteIntervals() {
      while (broadcastInterval(true));
   }

   private boolean broadcastInterval(boolean broadcastCompleteIntervalsOnly) {
      Iterator<ActivityType> activityTypeIterator = intervalMesgs.keySet()
            .iterator();
      HashMap<ActivityType, MonitoringMesg> broadcastMesgs = new HashMap<ActivityType, MonitoringMesg>();
      MonitoringMesg allActivityBroadcastMesg = null;
      MonitoringMesg allActivityTotals;

      if (endTimestamp == lastTimestamp)
         return false; // Already broadcast all pending data.

      // Adjust start timestamp from start of file to start of day for daily total output.
	  if (outputDailyTotals)
         startTimestamp = modTimestampToLocalInterval(startTimestamp);

      // Initialize end timestamp if required.
      if (endTimestamp < startTimestamp)
         endTimestamp = modTimestampToLocalInterval(startTimestamp);
      
      // If the last data timestamp is within the next interval then this interval is incomplete.
	  if (broadcastCompleteIntervalsOnly && ((endTimestamp + interval) > lastTimestamp))
         return false;

      // Broadcast to end of interval.
      endTimestamp += interval;
      
      // If start timestamp is already at end of interval then broadcast all pending data to the last message received.
      // This is the end of file case when broadcast() is called to flush all data.
      if (endTimestamp > lastTimestamp)
         endTimestamp = lastTimestamp;

      while (activityTypeIterator.hasNext()) {
         ActivityType activityType = activityTypeIterator.next();
         ArrayList<MonitoringMesg> mesgList = intervalMesgs.get(activityType);
         MonitoringMesg mesg = computeInterval(activityType, mesgList);
         int i;

         if (mesg != null) {
            broadcastMesgs.put(activityType, mesg);

            if (mesg.getActivityType() == ActivityType.ALL) {
               allActivityBroadcastMesg = mesg;
            }
         }

         // Initialize for next interval by removing all messages up to the end of this interval.
         // One message at or before the end of the interval is retained to initialize the start of the next interval.
         i = 0;
         while (i < mesgList.size()) {
            if (mesgList.get(i).getTimestamp().getTimestamp() > endTimestamp)
               break;

            i++;
         }
         i--; // Decrement to point to the start message.
         while (i > 0) {
            mesgList.remove(--i);
         }
      }

      if (broadcastMesgs.size() > 0) {
         MonitoringMesg mesg = broadcastMesgs.values().iterator().next();

         // Compute totals for all activity.
         allActivityTotals = new MonitoringMesg();
         allActivityTotals.setTimestamp(mesg.getTimestamp());
         allActivityTotals.setLocalTimestamp(mesg.getLocalTimestamp());
         allActivityTotals.setActivityType(ActivityType.ALL);
         allActivityTotals.setDuration(mesg.getDuration());
         activityTypeIterator = broadcastMesgs.keySet().iterator();

         while (activityTypeIterator.hasNext()) {
            ActivityType activityType = activityTypeIterator.next();
            mesg = broadcastMesgs.get(activityType);

            if (mesg.getActivityType() != ActivityType.ALL) {
               for (String fieldName : accumulatedFieldNames) {
                  if (mesg.getFieldDoubleValue(fieldName) != null) {
                     if (allActivityTotals.getFieldDoubleValue(fieldName) == null) {
                        allActivityTotals.setFieldValue(fieldName, 0.0f);
                     }

                     allActivityTotals.setFieldValue(fieldName, allActivityTotals.getFieldDoubleValue(fieldName)
                           + mesg.getFieldDoubleValue(fieldName));
                  }
               }
            }
         }

         // Compute total calories if not logged by device.
         if (allActivityTotals.getCalories() == null) {
            if (infoMesg.getRestingMetabolicRate() != null) {
               allActivityTotals.setCalories((int) (allActivityTotals.getDuration() * infoMesg.getRestingMetabolicRate() / (24 * 3600)));
               
               if (allActivityTotals.getActiveCalories() != null) {
                  allActivityTotals.setCalories(allActivityTotals.getCalories() + allActivityTotals.getActiveCalories());
               }
            }
         }

         // Add the totals to the broadcast messages.
         // If the device logged data for all activity then only set computed
         // totals for fields that are not logged by the device (don't override
         // device data).
         if (allActivityBroadcastMesg != null) {
            for (Field field : allActivityTotals.fields) {
               if (allActivityBroadcastMesg.getField(field.num) == null) {
                  allActivityBroadcastMesg.setField(field);
               }
            }
         } else {
            broadcastMesgs.put(ActivityType.ALL, allActivityTotals);
         }

         // Broadcast messages to listeners.
         activityTypeIterator = broadcastMesgs.keySet().iterator();
         while (activityTypeIterator.hasNext()) {
            ActivityType activityType = activityTypeIterator.next();

            for (final MonitoringMesgListener listener : listeners) {
               listener.onMesg(broadcastMesgs.get(activityType));
            }
         }

         // Initialize for next interval.
         startTimestamp = endTimestamp;
      }
      
      return true;
   }

   public void onMesg(final MonitoringInfoMesg mesg) {
      DateTime utcTimestamp;
      LocalDateTime localTimestamp;

      infoMesg = mesg;
      utcTimestamp = infoMesg.getTimestamp();
      mesgTimestamp = utcTimestamp.getTimestamp();
      utcTimestamp.convertSystemTimeToUTC(systemToUtcTimestampOffset);
      infoMesg.setTimestamp(utcTimestamp);
      lastTimestamp = utcTimestamp.getTimestamp();

      if (infoMesg.getLocalTimestamp() != null) {
         localTimestamp = new LocalDateTime(infoMesg.getLocalTimestamp());
         localTimestamp.convertSystemTimeToLocal(systemToLocalTimestampOffset);
         localTimeOffset = localTimestamp.getTimestamp() - lastTimestamp;
      } else {
         localTimeOffset = systemToLocalTimestampOffset
               - systemToUtcTimestampOffset;
      }

      startTimestamp = lastTimestamp;
   }

   public void onMesg(final MonitoringMesg mesg) {
      MonitoringMesg nextMesg;
      MonitoringMesg lastMesg;
      ArrayList<MonitoringMesg> intervalMesgList;
      MonitoringMesg intervalMesg;

      if (infoMesg == null)
         return; // Can't process monitoring data messages without info
                 // message.

      nextMesg = extract(mesg);

      // If activity type is not specified then the data applies to all.
      if (nextMesg.getActivityType() == null)
         nextMesg.setActivityType(ActivityType.ALL);

      // Ignore messages with no timestamp field (invalid).
      if (nextMesg.getTimestamp() == null)
         return;

      // Wait for the next message with a different timestamp before
      // processing the last message because there can be multiple messages
      // (activity types) with the same timestamp.
      if (lastTimestamp != nextMesg.getTimestamp().getTimestamp()) {
         // If we have all the messages for this interval.
         if ((lastTimestamp - modTimestampToLocalInterval(startTimestamp)) >= interval) {
            broadcastCompleteIntervals();
         }
      }

      // If the current activity type is logged then accumulated values for other activity types have not changed last since message.
      // This is an implied start for other activity types so insert a message with last known accumulated values.
      if (mesg.getCurrentActivityTypeIntensity() != null) {
         lastMesg = lastMesgs.get(nextMesg.getActivityType());
         
         if (lastMesg == null) {
            intervalMesgList = new ArrayList<MonitoringMesg>();
            intervalMesg = new MonitoringMesg();
            intervalMesg.setActivityType(nextMesg.getActivityType());
            intervalMesg.setTimestamp(new DateTime(nextMesg.getTimestamp().getTimestamp() - nextMesg.getActiveTime().longValue()));
            intervalMesgList.add(intervalMesg);
            intervalMesgs.put(intervalMesg.getActivityType(), intervalMesgList);
         }
         
         for (MonitoringMesg otherActivityTypelastMesg : lastMesgs.values()) {
            if (otherActivityTypelastMesg.getActivityType() != nextMesg
                  .getActivityType()) {
               MonitoringMesg startMesg = new MonitoringMesg();
               startMesg.setTimestamp(nextMesg.getTimestamp());
               startMesg.setActivityType(otherActivityTypelastMesg.getActivityType());
               
               for (String fieldName : accumulatedFieldNames) {
                  if (otherActivityTypelastMesg.getField(fieldName) != null) {
                     startMesg.setField(otherActivityTypelastMesg.getField(fieldName));
                  }
               }

               intervalMesgs.get(startMesg.getActivityType()).add(startMesg);
            }
         }
      }

      // Save the last message for decoding of accumulated fields.
      lastTimestamp = nextMesg.getTimestamp().getTimestamp();
      lastMesg = lastMesgs.get(nextMesg.getActivityType());
      if (lastMesg == null) {
         lastMesg = new MonitoringMesg();
         lastMesgs.put(nextMesg.getActivityType(), lastMesg);
      }
      setFieldsFromMesg(lastMesg, nextMesg);

      // Add the next message to the list of messages in this interval.
      // Merge messages of the same activity type and timestamp.
      intervalMesgList = intervalMesgs
            .get(nextMesg.getActivityType());
      intervalMesg = null;

      if (intervalMesgList == null) {
         intervalMesgList = new ArrayList<MonitoringMesg>();
         intervalMesgs.put(nextMesg.getActivityType(), intervalMesgList);
      }

      if (intervalMesgList.size() > 0)
         intervalMesg = intervalMesgList.get(intervalMesgList.size() - 1);

      if ((intervalMesg != null)
            && nextMesg.getTimestamp().equals(intervalMesg.getTimestamp())) {
         setFieldsFromMesg(intervalMesg, nextMesg);
      } else {
         intervalMesgList.add(nextMesg);
      }
   }

   public void onMesg(DeviceSettingsMesg mesg) {
      if (mesg.getUtcOffset() != null) {
         int timeZoneIndex = 0;
         long offset = mesg.getUtcOffset();

         setSystemToUtcTimestampOffset(offset);

         if (mesg.getActiveTimeZone() != null)
            timeZoneIndex = mesg.getActiveTimeZone();

         if (mesg.getTimeZoneOffset(timeZoneIndex) != null)
            offset += (double) mesg.getTimeZoneOffset(timeZoneIndex) * 3600;

         setSystemToLocalTimestampOffset(offset);
      }
   }

   private long modTimestampToLocalInterval(long timestamp) {
      timestamp += localTimeOffset;
      timestamp -= timestamp % interval;
      timestamp -= localTimeOffset;
      return timestamp;
   }

   private void setFieldsFromMesg(MonitoringMesg destMesg,
         MonitoringMesg srcMesg) {
      Iterator<Field> fieldIterator = srcMesg.fields.iterator();

      while (fieldIterator.hasNext())
         destMesg.setField(new Field(fieldIterator.next()));
   }

   private MonitoringMesg extract(final MonitoringMesg in) {
      MonitoringMesg out = new MonitoringMesg();
      int activityTypeInfoIndex = Integer.MAX_VALUE;
      MonitoringMesg lastMesg = null;
      DateTime timestamp;
      ExtractState extractState;

      // Timestamp
      if (in.getTimestamp() != null) {
         mesgTimestamp = in.getTimestamp().getTimestamp();
      } else if (in.getTimestamp16() != null) {
         mesgTimestamp += (in.getTimestamp16() - (mesgTimestamp & 0xFFFF)) & 0xFFFF;
      } else if (in.getTimestampMin8() != null) {
         mesgTimestamp /= 60; // Truncate to nearest minute.
         mesgTimestamp += (in.getTimestampMin8() - (mesgTimestamp & 0xFF)) & 0xFF;
         mesgTimestamp *= 60; // Back to seconds.
      }
      timestamp = new DateTime(mesgTimestamp);
      timestamp.convertSystemTimeToUTC(systemToUtcTimestampOffset);
      out.setTimestamp(timestamp);

      if (in.getLocalTimestamp() != null) {
         out.setLocalTimestamp(in.getLocalTimestamp());
      } else {
         out.setLocalTimestamp(timestamp.getTimestamp() + localTimeOffset);
      }

      // Activity Type
      if (in.getActivityType() != null) {
         out.setActivityType(in.getActivityType());
      }

      // Get extraction state for this activity type.
      extractState = extractStates.get(out.getActivityType());
      if (extractState == null) {
         extractState = new ExtractState();
         extractStates.put(out.getActivityType(), extractState);
      }

      // Get index for activity info, if any. (cycles to distance/calories scale
      // factors).
      if (infoMesg.getNumActivityType() > 0) {
         for (int i = 0; i < infoMesg.getNumActivityType(); i++) {
            if (infoMesg.getActivityType(i) == out.getActivityType()) {
               activityTypeInfoIndex = i;
            }
         }
      }

      // Get the last message for decoding rolling over accumulated fields.
      lastMesg = lastMesgs.get(in.getActivityType());

      if (lastMesg == null)
         lastMesg = new MonitoringMesg();

      // Duration
      if (in.getDuration() != null) {
         out.setDuration(in.getDuration());
      } else if (in.getDurationMin() != null) {
         out.setDuration((long)in.getDurationMin() * 60);
      }

      // Active time
      if (in.getActiveTime() != null) {
         out.setActiveTime(in.getActiveTime());
      } else if (in.getActiveTime16() != null) {
         long time = 0;

         if (lastMesg.getActiveTime() != null) {
            time = (long) (lastMesg.getActiveTime() + 0.5);
         }

         time += (in.getActiveTime16() - (time & 0xFFFF)) & 0xFFFF;
         out.setActiveTime((float) time);
      } else if (in.getCurrentActivityTypeIntensity() != null) {
         // If this is the current activity type then time since last message is
         // active time in the current activity type.
         long time = 0;

         if (lastMesg.getActiveTime() != null) {
            time = (long) (lastMesg.getActiveTime() + 0.5);
         }

         time += timestamp.getTimestamp() - lastTimestamp;
         out.setActiveTime((float) time);
      }

      // Cycles
      if (in.getCycles() != null) {
         out.setCycles(in.getCycles());
      } else if (in.getCycles16() != null) {
         long cycles = 0;

         if (lastMesg.getCycles() != null) {
            cycles = (long) (lastMesg.getCycles() * 2);
         }

         cycles += (in.getCycles16() - (cycles & 0xFFFF)) & 0xFFFF;
         out.setCycles((float) cycles / 2);
      }

      // Distance
      if (in.getDistance() != null) {
         out.setDistance(in.getDistance());
      } else if (in.getDistance16() != null) {
         long distance = 0;

         if (lastMesg.getDistance() != null) {
            distance = (long) (lastMesg.getDistance() * 100);
         }

         distance += (in.getDistance16() - (distance & 0xFFFF)) & 0xFFFF;
         out.setDistance((float) distance / 100);
      }

      // Active Calories
      if (in.getActiveCalories() != null) {
         out.setActiveCalories(in.getActiveCalories());
      }

      // Total Calories
      if (in.getCalories() != null) {
         out.setCalories(in.getCalories());
      }

      // Intensity
      if ((in.getIntensity() != null)) {
         out.setIntensity(in.getIntensity());
      }

      // Heart Rate
      if ((in.getHeartRate() != null)) {
         out.setHeartRate(in.getHeartRate());
      }

      // Temperature
      if ((in.getTemperature() != null)) {
         out.setTemperature(in.getTemperature());
      }

      // Compute distance from cycles if not logged directly.
      if (out.getDistance() != null) {
         // Keep track of cycles at last logged distance to compute distance
         // from cycles.
         extractState.cyclesToDistanceStartDist = out.getDistance();
         extractState.cyclesToDistanceStartCycles = out.getCycles();
      } else if ((activityTypeInfoIndex < infoMesg.getNumCyclesToDistance())
            && (out.getCycles() != null)) {
         // Compute distance from cycles since last reported distance.
         // Distance is computed from total cycles since last reported
         // distance instead of accumulating computed distance which would
         // accumulate error.
         out.setDistance(extractState.cyclesToDistanceStartDist
               + (out.getCycles() - extractState.cyclesToDistanceStartCycles)
               * infoMesg.getCyclesToDistance(activityTypeInfoIndex));
      }

      // Compute active calories from cycles if not logged directly.
      if (out.getActiveCalories() != null) {
         // Keep track of cycles at last logged calories to compute calories
         // from cycles.
         extractState.cyclesToCaloriesStartCal = out.getActiveCalories();
         extractState.cyclesToCaloriesStartCycles = out.getCycles();
      } else if ((activityTypeInfoIndex < infoMesg.getNumCyclesToCalories())
            && (out.getCycles() != null)) {
         // Compute calories from cycles since last reported calories.
         // Calories is computed from total cycles since last reported
         // calories instead of accumulating computed calories which would
         // accumulate error.
         out.setActiveCalories((int) (extractState.cyclesToCaloriesStartCal + (out
               .getCycles() - extractState.cyclesToCaloriesStartCycles)
               * infoMesg.getCyclesToCalories(activityTypeInfoIndex)));
      }

      return out;
   }

   private MonitoringMesg computeInterval(ActivityType activityType,
         ArrayList<MonitoringMesg> intervalMesgs) {
      MonitoringMesg intervalMesg = new MonitoringMesg();
      ArrayList<ReaderField> fields = new ArrayList<ReaderField>();
      boolean intervalHasData = false;
      boolean mesgInInterval = false;

      if (intervalMesgs.size() == 0)
         return null;

      intervalMesg.setTimestamp(new DateTime(endTimestamp));
      intervalMesg.setLocalTimestamp(endTimestamp + localTimeOffset);
      intervalMesg.setActivityType(activityType);
      intervalMesg.setDuration(endTimestamp - startTimestamp);

      for (String fieldName : accumulatedFieldNames) {
         fields.add(new AccumField(MonitoringMesg.monitoringMesg.getField(fieldName)));
      }
      
      for (String fieldName : instantaneousFieldNames) {
         fields.add(new InstField(MonitoringMesg.monitoringMesg.getField(fieldName)));
      }

      for (MonitoringMesg mesg : intervalMesgs) {
         long mesgTimestamp = mesg.getTimestamp().getTimestamp();

         if ((mesgTimestamp > startTimestamp)
               && (mesgTimestamp < (endTimestamp + interval)))
            mesgInInterval = true;
         
         for (ReaderField field : fields) {
            field.onMesg(mesg);
         }
      }
      
      if (!mesgInInterval) {
         return null;
      }

      for (ReaderField field : fields) {
         if (field.setMesg(intervalMesg))
            intervalHasData = true;
      }
      
      if (!intervalHasData)
         return null;

      return intervalMesg;
   }

   private interface ReaderField extends MonitoringMesgListener {
      public boolean setMesg(MonitoringMesg mesg);
   }

   private class AccumField extends Field implements ReaderField {
      private Double startValue;
      private long startValueTimestamp;
      private Double endValue;
      private long endValueTimestamp;

      public AccumField(Field field) {
         super(field);
         
         // Initialize start value to start of day which is defined as 0.
         startValue = new Double(0);
         startValueTimestamp = startTimestamp + localTimeOffset;
         startValueTimestamp -= startValueTimestamp % 86400;
         startValueTimestamp -= localTimeOffset;
         
         endValue = null;
         endValueTimestamp = 0;
      }

      public boolean setMesg(MonitoringMesg mesg) {
         double value;

         if (endValue == null)
            return false;

         value = endValue - startValue;

         // Accumulated difference should always be positive.
         // If it is negative there is probably a error in the data logged by
         // the device.
         // Set the difference to zero and let the accumulated value catch up.
         if (value < 0)
            value = 0;

         mesg.setFieldValue(this.num, 0, value);
         
         return value != 0;
      }

      public void onMesg(MonitoringMesg mesg) {
         Field field = mesg.getField(this.num);
         long mesgTimestamp = mesg.getTimestamp().getTimestamp();
         Double value = null;

         if (field != null)
            value = field.getDoubleValue();
         
         if (mesgTimestamp <= startTimestamp) {
            if (value != null) 
               startValue = value;
                  
            startValueTimestamp = mesgTimestamp;
         } else {
            // Interpolate start value if not aligned to start of interval.
            if (startValueTimestamp < startTimestamp) {
               if (value != null)
                  startValue = startValue + (value - startValue) * (startTimestamp - startValueTimestamp) / (mesgTimestamp - startValueTimestamp);

               startValueTimestamp = startTimestamp;
            }
            
            // Interpolate end of interval if message data spans into next
            // interval.
            if (mesgTimestamp > endTimestamp) {
               if (value != null) {
                  if (endValue == null) {
                     endValue = startValue;
                     endValueTimestamp = startValueTimestamp;
                  }
                  
                  value = endValue + (value - endValue) * (endTimestamp - endValueTimestamp) / (mesgTimestamp - endValueTimestamp);
               }
            }            

            endValueTimestamp = mesgTimestamp;
            
            if (value != null)
               endValue = value;
         }
      }
   }

   private class InstField extends Field implements ReaderField {
      private double sum;
      private double sumDuration;
      private long sumEndTimestamp;

      public InstField(Field field) {
         super(field);
         sum = 0;
         sumDuration = 0;
         sumEndTimestamp = startTimestamp;
      }

      public boolean setMesg(MonitoringMesg mesg) {
         if (sumDuration == 0)
            return false;

         mesg.setFieldValue(this.num, 0, sum / sumDuration);
         
         return true;
      }

      public void onMesg(MonitoringMesg mesg) {
         Field field = mesg.getField(this.num);
         Double value;
         double mesgDuration;
         long mesgTimestamp;

         // Ignore instantaneous data if outputting daily totals.
         // Instantaneous fields can not be computed for daily interval without data for the whole day.
         if (outputDailyTotals)
            return;

         mesgTimestamp = mesg.getTimestamp().getTimestamp();

         if (mesgTimestamp > endTimestamp)
            mesgTimestamp = endTimestamp;

         if (sumEndTimestamp >= mesgTimestamp)
            return;
         
         mesgDuration = mesgTimestamp - sumEndTimestamp;
         sumEndTimestamp = mesgTimestamp;

         if (field == null)
            return;

         value = field.getDoubleValue();

         if (value == null)
            return;

         sumDuration += mesgDuration;
         sum += value * mesgDuration;
      }
   }
}
