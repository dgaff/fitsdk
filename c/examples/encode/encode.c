////////////////////////////////////////////////////////////////////////////////
// The following FIT Protocol software provided may be used with FIT protocol
// devices only and remains the copyrighted property of Dynastream Innovations Inc.
// The software is being provided on an "as-is" basis and as an accommodation,
// and therefore all warranties, representations, or guarantees of any kind
// (whether express, implied or statutory) including, without limitation,
// warranties of merchantability, non-infringement, or fitness for a particular
// purpose, are specifically disclaimed.
//
// Copyright 2008 Dynastream Innovations Inc.
////////////////////////////////////////////////////////////////////////////////

#define _CRT_SECURE_NO_WARNINGS

#include "stdio.h"
#include "string.h"

#include "fit_product.h"
#include "fit_crc.h"

///////////////////////////////////////////////////////////////////////
// Private Function Prototypes
///////////////////////////////////////////////////////////////////////

void WriteFileHeader(FILE *fp);
///////////////////////////////////////////////////////////////////////
// Creates a FIT file. Puts a place-holder for the file header on top of the file.
///////////////////////////////////////////////////////////////////////

void WriteMessageDefinition(FIT_UINT8 local_mesg_number, const void *mesg_def_pointer, FIT_UINT8 mesg_def_size, FILE *fp);
///////////////////////////////////////////////////////////////////////
// Appends a FIT message definition (including the definition header) to the end of a file.
///////////////////////////////////////////////////////////////////////

void WriteMessage(FIT_UINT8 local_mesg_number, const void *mesg_pointer, FIT_UINT8 mesg_size, FILE *fp);
///////////////////////////////////////////////////////////////////////
// Appends a FIT message (including the message header) to the end of a file.
///////////////////////////////////////////////////////////////////////

void WriteData(const void *data, FIT_UINT8 data_size, FILE *fp);
///////////////////////////////////////////////////////////////////////
// Writes data to the file and updates the data CRC.
///////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////
// Private Variables
///////////////////////////////////////////////////////////////////////

static FIT_UINT16 data_crc;


int main(void)
{
   FILE *fp;

   data_crc = 0;
   fp = fopen("test.fit", "w+b");

   WriteFileHeader(fp);

   // Write file id message.
   {
      FIT_UINT8 file_id_local_mesg_number = 0;
      FIT_FILE_ID_MESG file_id;
      Fit_InitMesg(fit_mesg_defs[FIT_MESG_FILE_ID], &file_id);
      file_id.type = FIT_FILE_SETTINGS;
      file_id.manufacturer = FIT_MANUFACTURER_GARMIN;
      WriteMessageDefinition(file_id_local_mesg_number, fit_mesg_defs[FIT_MESG_FILE_ID], FIT_FILE_ID_MESG_DEF_SIZE, fp);
      WriteMessage(file_id_local_mesg_number, &file_id, FIT_FILE_ID_MESG_SIZE, fp);
   }

   // Write user profile message.
   {
      FIT_UINT8 profile_local_mesg_number = 1; // In some cases, careful selection of local message numbers may reduce number of definition messages in a FIT file
      FIT_USER_PROFILE_MESG user_profile;
      Fit_InitMesg(fit_mesg_defs[FIT_MESG_USER_PROFILE], &user_profile);
      user_profile.gender = FIT_GENDER_FEMALE;
      user_profile.age = 35;
      WriteMessageDefinition(profile_local_mesg_number, fit_mesg_defs[FIT_MESG_USER_PROFILE], FIT_USER_PROFILE_MESG_DEF_SIZE, fp);
      WriteMessage(profile_local_mesg_number, &user_profile, FIT_USER_PROFILE_MESG_SIZE, fp);
   }

   // Write CRC.
   fwrite(&data_crc, 1, sizeof(FIT_UINT16), fp);

   // Update file header with data size.
   WriteFileHeader(fp);

   fclose(fp);

   return 0;
}

void WriteFileHeader(FILE *fp)
{
   FIT_FILE_HDR file_header;

   file_header.header_size = FIT_FILE_HDR_SIZE;
   file_header.profile_version = FIT_PROFILE_VERSION;
   file_header.protocol_version = FIT_PROTOCOL_VERSION;
   memcpy((FIT_UINT8 *)&file_header.data_type, ".FIT", 4);
   fseek (fp , 0 , SEEK_END);
   file_header.data_size = ftell(fp) - FIT_FILE_HDR_SIZE - sizeof(FIT_UINT16);
   file_header.crc = FitCRC_Calc16(&file_header, FIT_STRUCT_OFFSET(crc, FIT_FILE_HDR));

   fseek (fp , 0 , SEEK_SET);
   fwrite((void *)&file_header, 1, FIT_FILE_HDR_SIZE, fp);
}

void WriteMessageDefinition(FIT_UINT8 local_mesg_number, const void *mesg_def_pointer, FIT_UINT8 mesg_def_size, FILE *fp)
{
   FIT_UINT8 header = local_mesg_number | FIT_HDR_TYPE_DEF_BIT;
   WriteData(&header, FIT_HDR_SIZE, fp);
   WriteData(mesg_def_pointer, mesg_def_size, fp);
}

void WriteMessage(FIT_UINT8 local_mesg_number, const void *mesg_pointer, FIT_UINT8 mesg_size, FILE *fp)
{
   WriteData(&local_mesg_number, FIT_HDR_SIZE, fp);
   WriteData(mesg_pointer, mesg_size, fp);
}

void WriteData(const void *data, FIT_UINT8 data_size, FILE *fp)
{
   FIT_UINT8 offset;

   fwrite(data, 1, data_size, fp);

   for (offset = 0; offset < data_size; offset++)
      data_crc = FitCRC_Get16(data_crc, *((FIT_UINT8 *)data + offset));
}
