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


#if !defined(FIT_WORKOUT_STEP_MESG_HPP)
#define FIT_WORKOUT_STEP_MESG_HPP

#include "fit_mesg.hpp"

namespace fit
{

class WorkoutStepMesg : public Mesg
{
   public:
      WorkoutStepMesg(void) : Mesg(Profile::MESG_WORKOUT_STEP)
      {
      }

      WorkoutStepMesg(const Mesg &mesg) : Mesg(mesg)
      {
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns message_index field
      ///////////////////////////////////////////////////////////////////////
      FIT_MESSAGE_INDEX GetMessageIndex(void) const
      {
         return GetFieldUINT16Value(254, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set message_index field
      ///////////////////////////////////////////////////////////////////////
      void SetMessageIndex(FIT_MESSAGE_INDEX messageIndex)
      {
         SetFieldUINT16Value(254, messageIndex, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns wkt_step_name field
      ///////////////////////////////////////////////////////////////////////
      FIT_WSTRING GetWktStepName(void) const
      {
         return GetFieldSTRINGValue(0, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set wkt_step_name field
      ///////////////////////////////////////////////////////////////////////
      void SetWktStepName(FIT_WSTRING wktStepName)
      {
         SetFieldSTRINGValue(0, wktStepName, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_type field
      ///////////////////////////////////////////////////////////////////////
      FIT_WKT_STEP_DURATION GetDurationType(void) const
      {
         return GetFieldENUMValue(1, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_type field
      ///////////////////////////////////////////////////////////////////////
      void SetDurationType(FIT_WKT_STEP_DURATION durationType)
      {
         SetFieldENUMValue(1, durationType, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_value field
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetDurationValue(void) const
      {
         return GetFieldUINT32Value(2, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_value field
      ///////////////////////////////////////////////////////////////////////
      void SetDurationValue(FIT_UINT32 durationValue)
      {
         SetFieldUINT32Value(2, durationValue, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_time field
      // Units: s
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetDurationTime(void) const
      {
         return GetFieldFLOAT32Value(2, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_TIME);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_time field
      // Units: s
      ///////////////////////////////////////////////////////////////////////
      void SetDurationTime(FIT_FLOAT32 durationTime)
      {
         SetFieldFLOAT32Value(2, durationTime, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_TIME);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_distance field
      // Units: m
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetDurationDistance(void) const
      {
         return GetFieldFLOAT32Value(2, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_DISTANCE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_distance field
      // Units: m
      ///////////////////////////////////////////////////////////////////////
      void SetDurationDistance(FIT_FLOAT32 durationDistance)
      {
         SetFieldFLOAT32Value(2, durationDistance, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_DISTANCE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_hr field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_HR GetDurationHr(void) const
      {
         return GetFieldUINT32Value(2, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_HR);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_hr field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      void SetDurationHr(FIT_WORKOUT_HR durationHr)
      {
         SetFieldUINT32Value(2, durationHr, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_HR);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_calories field
      // Units: calories
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetDurationCalories(void) const
      {
         return GetFieldUINT32Value(2, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_CALORIES);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_calories field
      // Units: calories
      ///////////////////////////////////////////////////////////////////////
      void SetDurationCalories(FIT_UINT32 durationCalories)
      {
         SetFieldUINT32Value(2, durationCalories, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_CALORIES);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_step field
      // Comment: message_index of step to loop back to. Steps are assumed to be in the order by message_index. custom_name and intensity members are undefined for this duration type.
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetDurationStep(void) const
      {
         return GetFieldUINT32Value(2, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_STEP);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_step field
      // Comment: message_index of step to loop back to. Steps are assumed to be in the order by message_index. custom_name and intensity members are undefined for this duration type.
      ///////////////////////////////////////////////////////////////////////
      void SetDurationStep(FIT_UINT32 durationStep)
      {
         SetFieldUINT32Value(2, durationStep, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_STEP);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns duration_power field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_POWER GetDurationPower(void) const
      {
         return GetFieldUINT32Value(2, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_POWER);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set duration_power field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      void SetDurationPower(FIT_WORKOUT_POWER durationPower)
      {
         SetFieldUINT32Value(2, durationPower, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_DURATION_VALUE_FIELD_DURATION_POWER);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns target_type field
      ///////////////////////////////////////////////////////////////////////
      FIT_WKT_STEP_TARGET GetTargetType(void) const
      {
         return GetFieldENUMValue(3, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set target_type field
      ///////////////////////////////////////////////////////////////////////
      void SetTargetType(FIT_WKT_STEP_TARGET targetType)
      {
         SetFieldENUMValue(3, targetType, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns target_value field
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetTargetValue(void) const
      {
         return GetFieldUINT32Value(4, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set target_value field
      ///////////////////////////////////////////////////////////////////////
      void SetTargetValue(FIT_UINT32 targetValue)
      {
         SetFieldUINT32Value(4, targetValue, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns target_hr_zone field
      // Comment: hr zone (1-5);Custom =0;
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetTargetHrZone(void) const
      {
         return GetFieldUINT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_TARGET_HR_ZONE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set target_hr_zone field
      // Comment: hr zone (1-5);Custom =0;
      ///////////////////////////////////////////////////////////////////////
      void SetTargetHrZone(FIT_UINT32 targetHrZone)
      {
         SetFieldUINT32Value(4, targetHrZone, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_TARGET_HR_ZONE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns target_power_zone field
      // Comment: Power Zone ( 1-7); Custom = 0;
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetTargetPowerZone(void) const
      {
         return GetFieldUINT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_TARGET_POWER_ZONE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set target_power_zone field
      // Comment: Power Zone ( 1-7); Custom = 0;
      ///////////////////////////////////////////////////////////////////////
      void SetTargetPowerZone(FIT_UINT32 targetPowerZone)
      {
         SetFieldUINT32Value(4, targetPowerZone, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_TARGET_POWER_ZONE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns repeat_steps field
      // Comment: # of repetitions
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetRepeatSteps(void) const
      {
         return GetFieldUINT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_STEPS);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set repeat_steps field
      // Comment: # of repetitions
      ///////////////////////////////////////////////////////////////////////
      void SetRepeatSteps(FIT_UINT32 repeatSteps)
      {
         SetFieldUINT32Value(4, repeatSteps, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_STEPS);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns repeat_time field
      // Units: s
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetRepeatTime(void) const
      {
         return GetFieldFLOAT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_TIME);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set repeat_time field
      // Units: s
      ///////////////////////////////////////////////////////////////////////
      void SetRepeatTime(FIT_FLOAT32 repeatTime)
      {
         SetFieldFLOAT32Value(4, repeatTime, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_TIME);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns repeat_distance field
      // Units: m
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetRepeatDistance(void) const
      {
         return GetFieldFLOAT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_DISTANCE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set repeat_distance field
      // Units: m
      ///////////////////////////////////////////////////////////////////////
      void SetRepeatDistance(FIT_FLOAT32 repeatDistance)
      {
         SetFieldFLOAT32Value(4, repeatDistance, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_DISTANCE);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns repeat_calories field
      // Units: calories
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetRepeatCalories(void) const
      {
         return GetFieldUINT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_CALORIES);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set repeat_calories field
      // Units: calories
      ///////////////////////////////////////////////////////////////////////
      void SetRepeatCalories(FIT_UINT32 repeatCalories)
      {
         SetFieldUINT32Value(4, repeatCalories, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_CALORIES);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns repeat_hr field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_HR GetRepeatHr(void) const
      {
         return GetFieldUINT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_HR);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set repeat_hr field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      void SetRepeatHr(FIT_WORKOUT_HR repeatHr)
      {
         SetFieldUINT32Value(4, repeatHr, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_HR);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns repeat_power field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_POWER GetRepeatPower(void) const
      {
         return GetFieldUINT32Value(4, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_POWER);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set repeat_power field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      void SetRepeatPower(FIT_WORKOUT_POWER repeatPower)
      {
         SetFieldUINT32Value(4, repeatPower, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_TARGET_VALUE_FIELD_REPEAT_POWER);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_value_low field
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetCustomTargetValueLow(void) const
      {
         return GetFieldUINT32Value(5, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_value_low field
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetValueLow(FIT_UINT32 customTargetValueLow)
      {
         SetFieldUINT32Value(5, customTargetValueLow, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_speed_low field
      // Units: m/s
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetCustomTargetSpeedLow(void) const
      {
         return GetFieldFLOAT32Value(5, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_SPEED_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_speed_low field
      // Units: m/s
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetSpeedLow(FIT_FLOAT32 customTargetSpeedLow)
      {
         SetFieldFLOAT32Value(5, customTargetSpeedLow, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_SPEED_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_heart_rate_low field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_HR GetCustomTargetHeartRateLow(void) const
      {
         return GetFieldUINT32Value(5, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_HEART_RATE_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_heart_rate_low field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetHeartRateLow(FIT_WORKOUT_HR customTargetHeartRateLow)
      {
         SetFieldUINT32Value(5, customTargetHeartRateLow, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_HEART_RATE_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_cadence_low field
      // Units: rpm
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetCustomTargetCadenceLow(void) const
      {
         return GetFieldUINT32Value(5, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_CADENCE_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_cadence_low field
      // Units: rpm
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetCadenceLow(FIT_UINT32 customTargetCadenceLow)
      {
         SetFieldUINT32Value(5, customTargetCadenceLow, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_CADENCE_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_power_low field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_POWER GetCustomTargetPowerLow(void) const
      {
         return GetFieldUINT32Value(5, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_POWER_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_power_low field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetPowerLow(FIT_WORKOUT_POWER customTargetPowerLow)
      {
         SetFieldUINT32Value(5, customTargetPowerLow, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_LOW_FIELD_CUSTOM_TARGET_POWER_LOW);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_value_high field
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetCustomTargetValueHigh(void) const
      {
         return GetFieldUINT32Value(6, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_value_high field
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetValueHigh(FIT_UINT32 customTargetValueHigh)
      {
         SetFieldUINT32Value(6, customTargetValueHigh, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_speed_high field
      // Units: m/s
      ///////////////////////////////////////////////////////////////////////
      FIT_FLOAT32 GetCustomTargetSpeedHigh(void) const
      {
         return GetFieldFLOAT32Value(6, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_SPEED_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_speed_high field
      // Units: m/s
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetSpeedHigh(FIT_FLOAT32 customTargetSpeedHigh)
      {
         SetFieldFLOAT32Value(6, customTargetSpeedHigh, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_SPEED_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_heart_rate_high field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_HR GetCustomTargetHeartRateHigh(void) const
      {
         return GetFieldUINT32Value(6, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_HEART_RATE_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_heart_rate_high field
      // Units: % or bpm
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetHeartRateHigh(FIT_WORKOUT_HR customTargetHeartRateHigh)
      {
         SetFieldUINT32Value(6, customTargetHeartRateHigh, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_HEART_RATE_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_cadence_high field
      // Units: rpm
      ///////////////////////////////////////////////////////////////////////
      FIT_UINT32 GetCustomTargetCadenceHigh(void) const
      {
         return GetFieldUINT32Value(6, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_CADENCE_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_cadence_high field
      // Units: rpm
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetCadenceHigh(FIT_UINT32 customTargetCadenceHigh)
      {
         SetFieldUINT32Value(6, customTargetCadenceHigh, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_CADENCE_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns custom_target_power_high field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      FIT_WORKOUT_POWER GetCustomTargetPowerHigh(void) const
      {
         return GetFieldUINT32Value(6, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_POWER_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set custom_target_power_high field
      // Units: % or watts
      ///////////////////////////////////////////////////////////////////////
      void SetCustomTargetPowerHigh(FIT_WORKOUT_POWER customTargetPowerHigh)
      {
         SetFieldUINT32Value(6, customTargetPowerHigh, 0, (FIT_UINT16) Profile::WORKOUT_STEP_MESG_CUSTOM_TARGET_VALUE_HIGH_FIELD_CUSTOM_TARGET_POWER_HIGH);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns intensity field
      ///////////////////////////////////////////////////////////////////////
      FIT_INTENSITY GetIntensity(void) const
      {
         return GetFieldENUMValue(7, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set intensity field
      ///////////////////////////////////////////////////////////////////////
      void SetIntensity(FIT_INTENSITY intensity)
      {
         SetFieldENUMValue(7, intensity, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

};

} // namespace fit

#endif // !defined(FIT_WORKOUT_STEP_MESG_HPP)
