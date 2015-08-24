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


#if !defined(FIT_MONITORING_INFO_MESG_HPP)
#define FIT_MONITORING_INFO_MESG_HPP

#include "fit_mesg.hpp"

namespace fit
{

class MonitoringInfoMesg : public Mesg
{
   public:
      MonitoringInfoMesg(void) : Mesg(Profile::MESG_MONITORING_INFO)
      {
      }

      MonitoringInfoMesg(const Mesg &mesg) : Mesg(mesg)
      {
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns timestamp field
      // Units: s
      ///////////////////////////////////////////////////////////////////////
      FIT_DATE_TIME GetTimestamp(void) const
      {
         return GetFieldUINT32Value(253, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set timestamp field
      // Units: s
      ///////////////////////////////////////////////////////////////////////
      void SetTimestamp(FIT_DATE_TIME timestamp)
      {
         SetFieldUINT32Value(253, timestamp, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns local_timestamp field
      // Units: s
      // Comment: Use to convert activity timestamps to local time if device does not support time zone and daylight savings time correction.
      ///////////////////////////////////////////////////////////////////////
      FIT_LOCAL_DATE_TIME GetLocalTimestamp(void) const
      {
         return GetFieldUINT32Value(0, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set local_timestamp field
      // Units: s
      // Comment: Use to convert activity timestamps to local time if device does not support time zone and daylight savings time correction.
      ///////////////////////////////////////////////////////////////////////
      void SetLocalTimestamp(FIT_LOCAL_DATE_TIME localTimestamp)
      {
         SetFieldUINT32Value(0, localTimestamp, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns number of activity_type
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT8 GetNumActivityType(void) const
      {
         return GetFieldNumValues(1, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns activity_type field
      ///////////////////////////////////////////////////////////////////////
      FIT_ACTIVITY_TYPE GetActivityType(FIT_UINT8 index) const
      {
         return GetFieldENUMValue(1, index, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set activity_type field
      ///////////////////////////////////////////////////////////////////////
      void SetActivityType(FIT_UINT8 index, FIT_ACTIVITY_TYPE activityType)
      {
         SetFieldENUMValue(1, activityType, index, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns number of cycles_to_distance
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT8 GetNumCyclesToDistance(void) const
      {
         return GetFieldNumValues(3, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns cycles_to_distance field
      // Units: m/cycle
      // Comment: Indexed by activity_type
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetCyclesToDistance(FIT_UINT8 index) const
      {
         return GetFieldFLOAT32Value(3, index, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set cycles_to_distance field
      // Units: m/cycle
      // Comment: Indexed by activity_type
      ///////////////////////////////////////////////////////////////////////
      void SetCyclesToDistance(FIT_UINT8 index, FIT_FLOAT32 cyclesToDistance)
      {
         SetFieldFLOAT32Value(3, cyclesToDistance, index, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns number of cycles_to_calories
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT8 GetNumCyclesToCalories(void) const
      {
         return GetFieldNumValues(4, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns cycles_to_calories field
      // Units: kcal/cycle
      // Comment: Indexed by activity_type
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetCyclesToCalories(FIT_UINT8 index) const
      {
         return GetFieldFLOAT32Value(4, index, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set cycles_to_calories field
      // Units: kcal/cycle
      // Comment: Indexed by activity_type
      ///////////////////////////////////////////////////////////////////////
      void SetCyclesToCalories(FIT_UINT8 index, FIT_FLOAT32 cyclesToCalories)
      {
         SetFieldFLOAT32Value(4, cyclesToCalories, index, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns resting_metabolic_rate field
      // Units: kcal / day
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT16 GetRestingMetabolicRate(void) const
      {
         return GetFieldUINT16Value(5, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set resting_metabolic_rate field
      // Units: kcal / day
      ///////////////////////////////////////////////////////////////////////
      void SetRestingMetabolicRate(FIT_UINT16 restingMetabolicRate)
      {
         SetFieldUINT16Value(5, restingMetabolicRate, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

};

} // namespace fit

#endif // !defined(FIT_MONITORING_INFO_MESG_HPP)
