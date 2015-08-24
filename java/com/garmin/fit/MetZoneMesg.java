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


public class MetZoneMesg extends Mesg {

   protected static final	Mesg metZoneMesg;
   static {         
      // met_zone   
      metZoneMesg = new Mesg("met_zone", MesgNum.MET_ZONE);
      metZoneMesg.addField(new Field("message_index", 254, 132, 1, 0, "", false));
      
      metZoneMesg.addField(new Field("high_bpm", 1, 2, 1, 0, "", false));
      
      metZoneMesg.addField(new Field("calories", 2, 132, 10, 0, "kcal / min", false));
      
      metZoneMesg.addField(new Field("fat_calories", 3, 2, 10, 0, "kcal / min", false));
      
   }

   public MetZoneMesg() {
      super(Factory.createMesg(MesgNum.MET_ZONE));
   }

   public MetZoneMesg(final Mesg mesg) {
      super(mesg);
   }


   /**
    * Get message_index field
    *
    * @return message_index
    */
   public Integer getMessageIndex() {
      return getFieldIntegerValue(254, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set message_index field
    *
    * @param messageIndex
    */
   public void setMessageIndex(Integer messageIndex) {
      setFieldValue(254, 0, messageIndex, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get high_bpm field
    *
    * @return high_bpm
    */
   public Short getHighBpm() {
      return getFieldShortValue(1, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set high_bpm field
    *
    * @param highBpm
    */
   public void setHighBpm(Short highBpm) {
      setFieldValue(1, 0, highBpm, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get calories field
    * Units: kcal / min
    *
    * @return calories
    */
   public Float getCalories() {
      return getFieldFloatValue(2, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set calories field
    * Units: kcal / min
    *
    * @param calories
    */
   public void setCalories(Float calories) {
      setFieldValue(2, 0, calories, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get fat_calories field
    * Units: kcal / min
    *
    * @return fat_calories
    */
   public Float getFatCalories() {
      return getFieldFloatValue(3, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set fat_calories field
    * Units: kcal / min
    *
    * @param fatCalories
    */
   public void setFatCalories(Float fatCalories) {
      setFieldValue(3, 0, fatCalories, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

}
