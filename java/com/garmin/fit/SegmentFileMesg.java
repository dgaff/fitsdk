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


public class SegmentFileMesg extends Mesg {

   protected static final	Mesg segmentFileMesg;
   static {         
      // segment_file   
      segmentFileMesg = new Mesg("segment_file", MesgNum.SEGMENT_FILE);
      segmentFileMesg.addField(new Field("message_index", 254, 132, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("file_uuid", 1, 7, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("enabled", 3, 0, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("user_profile_primary_key", 4, 134, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("leader_type", 7, 0, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("leader_group_primary_key", 8, 134, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("leader_activity_id", 9, 134, 1, 0, "", false));
      
      segmentFileMesg.addField(new Field("leader_activity_id_string", 10, 7, 1, 0, "", false));
      
   }

   public SegmentFileMesg() {
      super(Factory.createMesg(MesgNum.SEGMENT_FILE));
   }

   public SegmentFileMesg(final Mesg mesg) {
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
    * Get file_uuid field
    * Comment: UUID of the segment file
    *
    * @return file_uuid
    */
   public String getFileUuid() {
      return getFieldStringValue(1, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set file_uuid field
    * Comment: UUID of the segment file
    *
    * @param fileUuid
    */
   public void setFileUuid(String fileUuid) {
      setFieldValue(1, 0, fileUuid, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get enabled field
    * Comment: Enabled state of the segment file
    *
    * @return enabled
    */
   public Bool getEnabled() {
      Short value = getFieldShortValue(3, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
      if (value == null)
         return null;
      return Bool.getByValue(value);
   }

   /**
    * Set enabled field
    * Comment: Enabled state of the segment file
    *
    * @param enabled
    */
   public void setEnabled(Bool enabled) {
      setFieldValue(3, 0, enabled.value, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get user_profile_primary_key field
    * Comment: Primary key of the user that created the segment file
    *
    * @return user_profile_primary_key
    */
   public Long getUserProfilePrimaryKey() {
      return getFieldLongValue(4, 0, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set user_profile_primary_key field
    * Comment: Primary key of the user that created the segment file
    *
    * @param userProfilePrimaryKey
    */
   public void setUserProfilePrimaryKey(Long userProfilePrimaryKey) {
      setFieldValue(4, 0, userProfilePrimaryKey, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * @return number of leader_type
    */
   public int getNumLeaderType() {
      return getNumFieldValues(7, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get leader_type field
    * Comment: Leader type of each leader in the segment file
    *
    * @param index of leader_type
    * @return leader_type
    */
   public SegmentLeaderboardType getLeaderType(int index) {
      Short value = getFieldShortValue(7, index, Fit.SUBFIELD_INDEX_MAIN_FIELD);
      if (value == null)
         return null;
      return SegmentLeaderboardType.getByValue(value);
   }

   /**
    * Set leader_type field
    * Comment: Leader type of each leader in the segment file
    *
    * @param index of leader_type
    * @param leaderType
    */
   public void setLeaderType(int index, SegmentLeaderboardType leaderType) {
      setFieldValue(7, index, leaderType.value, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * @return number of leader_group_primary_key
    */
   public int getNumLeaderGroupPrimaryKey() {
      return getNumFieldValues(8, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get leader_group_primary_key field
    * Comment: Group primary key of each leader in the segment file
    *
    * @param index of leader_group_primary_key
    * @return leader_group_primary_key
    */
   public Long getLeaderGroupPrimaryKey(int index) {
      return getFieldLongValue(8, index, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set leader_group_primary_key field
    * Comment: Group primary key of each leader in the segment file
    *
    * @param index of leader_group_primary_key
    * @param leaderGroupPrimaryKey
    */
   public void setLeaderGroupPrimaryKey(int index, Long leaderGroupPrimaryKey) {
      setFieldValue(8, index, leaderGroupPrimaryKey, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * @return number of leader_activity_id
    */
   public int getNumLeaderActivityId() {
      return getNumFieldValues(9, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get leader_activity_id field
    * Comment: Activity ID of each leader in the segment file
    *
    * @param index of leader_activity_id
    * @return leader_activity_id
    */
   public Long getLeaderActivityId(int index) {
      return getFieldLongValue(9, index, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set leader_activity_id field
    * Comment: Activity ID of each leader in the segment file
    *
    * @param index of leader_activity_id
    * @param leaderActivityId
    */
   public void setLeaderActivityId(int index, Long leaderActivityId) {
      setFieldValue(9, index, leaderActivityId, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * @return number of leader_activity_id_string
    */
   public int getNumLeaderActivityIdString() {
      return getNumFieldValues(10, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Get leader_activity_id_string field
    * Comment: String version of the activity ID of each leader in the segment file. 21 characters long for each ID, express in decimal
    *
    * @param index of leader_activity_id_string
    * @return leader_activity_id_string
    */
   public String getLeaderActivityIdString(int index) {
      return getFieldStringValue(10, index, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

   /**
    * Set leader_activity_id_string field
    * Comment: String version of the activity ID of each leader in the segment file. 21 characters long for each ID, express in decimal
    *
    * @param index of leader_activity_id_string
    * @param leaderActivityIdString
    */
   public void setLeaderActivityIdString(int index, String leaderActivityIdString) {
      setFieldValue(10, index, leaderActivityIdString, Fit.SUBFIELD_INDEX_MAIN_FIELD);
   }

}
