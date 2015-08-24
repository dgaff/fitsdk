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

#include <fstream>
#include <cstdlib>

#include "fit_encode.hpp"
#include "fit_mesg_broadcaster.hpp"
#include "fit_file_id_mesg.hpp"
#include "fit_date_time.hpp"

int EncodeSettingsFile()
{
   fit::Encode encode;
   std::fstream file;

   file.open("ExampleSettings.fit", std::ios::in | std::ios::out | std::ios::binary | std::ios::trunc);

   if (!file.is_open())
   {
      printf("Error opening file ExampleSettings.fit\n");
      return -1;
   }

   fit::FileIdMesg fileIdMesg; // Every FIT file requires a File ID message
   fileIdMesg.SetType(FIT_FILE_SETTINGS);
   fileIdMesg.SetManufacturer(FIT_MANUFACTURER_DYNASTREAM);
   fileIdMesg.SetProduct(1000);
   fileIdMesg.SetSerialNumber(12345);

   fit::UserProfileMesg userProfileMesg;
   userProfileMesg.SetGender(FIT_GENDER_FEMALE);
   userProfileMesg.SetWeight((FIT_FLOAT32)63.1);
   userProfileMesg.SetAge(99);
   std::wstring wstring_name(L"TestUser");
   userProfileMesg.SetFriendlyName(wstring_name);

   encode.Open(file);
   encode.Write(fileIdMesg);
   encode.Write(userProfileMesg);

   if (!encode.Close())
   {
      printf("Error closing encode.\n");
      return -1;
   }
   file.close();

   printf("Encoded FIT file ExampleSettings.fit.\n");
   return 0;
}

int EncodeMonitoringFile()
{
   fit::Encode encode;
   std::fstream file;

   time_t current_time_unix = time(0);
   fit::DateTime initTime(current_time_unix);

   file.open("ExampleMonitoringFile.fit", std::ios::in | std::ios::out | std::ios::binary | std::ios::trunc);

   if (!file.is_open())
   {
      printf("Error opening file ExampleMonitoringFile.fit\n");
      return -1;
   }

   encode.Open(file);

   fit::FileIdMesg fileIdMesg; // Every FIT file requires a File ID message
   fileIdMesg.SetType(FIT_FILE_MONITORING_B);
   fileIdMesg.SetManufacturer(FIT_MANUFACTURER_DYNASTREAM);
   fileIdMesg.SetProduct(1001);
   fileIdMesg.SetSerialNumber(54321);

   encode.Write(fileIdMesg);

   fit::DeviceInfoMesg deviceInfoMesg;
   deviceInfoMesg.SetTimestamp(initTime.GetTimeStamp()); // Convert to FIT time and write timestamp.
   deviceInfoMesg.SetBatteryStatus(FIT_BATTERY_STATUS_GOOD);

   encode.Write(deviceInfoMesg);

   fit::MonitoringMesg monitoringMesg;

   // By default, each time a new message is written the Local Message Type 0 will be redefined to match the new message.
   // In this case,to avoid having a definition message each time there is a DeviceInfoMesg, we can manually set the Local Message Type of the MonitoringMessage to '1'.
   // By doing this we avoid an additional 7 definition messages in our FIT file.
   monitoringMesg.SetLocalNum(1);

   monitoringMesg.SetTimestamp(initTime.GetTimeStamp()); // Initialise Timestamp to now
   monitoringMesg.SetCycles(0); // Initialise Cycles to 0
   for(int i = 0; i < 4; i++) // This loop represents 1/6 of a day
   {
      for(int j = 0; j < 4; j++) // Each one of these loops represent 1 hour
      {
         fit::DateTime walkingTime(current_time_unix);
         monitoringMesg.SetTimestamp(walkingTime.GetTimeStamp());
         monitoringMesg.SetActivityType(FIT_ACTIVITY_TYPE_WALKING); // By setting this to WALKING, the Cycles field will be interpretted as Steps
         monitoringMesg.SetCycles(monitoringMesg.GetCycles() + (rand()%1000+1)); // Cycles are accumulated (i.e. must be increasing)
         encode.Write(monitoringMesg);
         current_time_unix += (time_t)(3600); //Add an hour to our contrieved timestamp
      }
      fit::DateTime statusTime(current_time_unix);
      deviceInfoMesg.SetTimestamp(statusTime.GetTimeStamp());
      deviceInfoMesg.SetBatteryStatus(FIT_BATTERY_STATUS_GOOD);
      encode.Write(deviceInfoMesg);

   }

   if (!encode.Close())
   {
      printf("Error closing encode.\n");
      return -1;
   }
   file.close();

   printf("Encoded FIT file ExampleMonitoringFile.fit.\n");
   return 0;
}

int main(int argc, char* argv[])
{
   printf("FIT Encode Example Application\n");

   int returnValue = 0;

   returnValue += EncodeSettingsFile();
   returnValue += EncodeMonitoringFile();

   return returnValue;
}
