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
#include <iostream>

#include "fit_decode.hpp"
#include "fit_mesg_broadcaster.hpp"

class Listener : public fit::FileIdMesgListener, public fit::UserProfileMesgListener, public fit::MonitoringMesgListener, public fit::DeviceInfoMesgListener, public fit::MesgListener
{
public :
   void OnMesg(fit::Mesg& mesg)
   {
      printf("On Mesg:\n");
      std::wcout << L"   New Mesg: " << mesg.GetName().c_str() << L".  It has " << mesg.GetNumFields() << L" field(s).\n";

      for (int i=0; i<mesg.GetNumFields(); i++)
      {
         fit::Field* field = mesg.GetFieldByIndex(i);
         std::wcout << L"   Field" << i << " (" << field->GetName().c_str() << ") has " << field->GetNumValues() << L" value(s)\n";
         for (int j=0; j<field->GetNumValues(); j++)
         {
            std::wcout << L"       Val" << j << L": ";
            switch (field->GetType())
            {
            case FIT_BASE_TYPE_ENUM:
               std::wcout << field->GetENUMValue(j);
               break;
            case FIT_BASE_TYPE_SINT8:
               std::wcout << field->GetSINT8Value(j);
               break;
            case FIT_BASE_TYPE_UINT8:
               std::wcout << field->GetUINT8Value(j);
               break;
            case FIT_BASE_TYPE_SINT16:
               std::wcout << field->GetSINT16Value(j);
               break;
            case FIT_BASE_TYPE_UINT16:
               std::wcout << field->GetUINT16Value(j);
               break;
            case FIT_BASE_TYPE_SINT32:
               std::wcout << field->GetSINT32Value(j);
               break;
            case FIT_BASE_TYPE_UINT32:
               std::wcout << field->GetUINT32Value(j);
               break;
            case FIT_BASE_TYPE_FLOAT32:
               std::wcout << field->GetFLOAT32Value(j);
               break;
            case FIT_BASE_TYPE_FLOAT64:
               std::wcout << field->GetFLOAT64Value(j);
               break;
            case FIT_BASE_TYPE_UINT8Z:
               std::wcout << field->GetUINT8ZValue(j);
               break;
            case FIT_BASE_TYPE_UINT16Z:
               std::wcout << field->GetUINT16ZValue(j);
               break;
            case FIT_BASE_TYPE_UINT32Z:
               std::wcout << field->GetUINT32ZValue(j);
               break;
            default:
               break;
            }
            //std::wcout << field->GetUnits().c_str() << L"\n";
            std::wcout << L"\n";
         }
      }
   }

   void OnMesg(fit::FileIdMesg& mesg)
   {
      printf("File ID:\n");
      if (mesg.GetType() != FIT_FILE_INVALID)
         printf("   Type: %d\n", mesg.GetType());
      if (mesg.GetManufacturer() != FIT_MANUFACTURER_INVALID)
         printf("   Manufacturer: %d\n", mesg.GetManufacturer());
      if (mesg.GetProduct() != FIT_UINT16_INVALID)
         printf("   Product: %d\n", mesg.GetProduct());
      if (mesg.GetSerialNumber() != FIT_UINT32Z_INVALID)
         printf("   Serial Number: %u\n", mesg.GetSerialNumber());
      if (mesg.GetNumber() != FIT_UINT16_INVALID)
         printf("   Number: %d\n", mesg.GetNumber());
   }

   void OnMesg(fit::UserProfileMesg& mesg)
   {
      printf("User profile:\n");
      if (mesg.GetFriendlyName() != FIT_WSTRING_INVALID)
         std::wcout << L"   Friendly Name: " << mesg.GetFriendlyName().c_str() << L"\n";
      if (mesg.GetGender() == FIT_GENDER_MALE)
         printf("   Gender: Male\n");
      if (mesg.GetGender() == FIT_GENDER_FEMALE)
         printf("   Gender: Female\n");
      if (mesg.GetAge() != FIT_UINT8_INVALID)
         printf("   Age [years]: %d\n", mesg.GetAge());
      if (mesg.GetWeight() != FIT_FLOAT32_INVALID)
         printf("   Weight [kg]: %0.2f\n", mesg.GetWeight());
   }

   void OnMesg(fit::DeviceInfoMesg& mesg)
   {
      printf("Device info:\n");

      if (mesg.GetTimestamp() != FIT_UINT32_INVALID)
         printf("   Timestamp: %d\n", mesg.GetTimestamp());

      switch(mesg.GetBatteryStatus())
      {
      case FIT_BATTERY_STATUS_CRITICAL:
         printf("   Battery status: Critical\n");
         break;
      case FIT_BATTERY_STATUS_GOOD:
         printf("   Battery status: Good\n");
         break;
      case FIT_BATTERY_STATUS_LOW:
         printf("   Battery status: Low\n");
         break;
      case FIT_BATTERY_STATUS_NEW:
         printf("   Battery status: New\n");
         break;
      case FIT_BATTERY_STATUS_OK:
         printf("   Battery status: OK\n");
         break;
      default:
         printf("   Battery status: Invalid\n");
         break;
      }
   }

   void OnMesg(fit::MonitoringMesg& mesg)
   {
      printf("Monitoring:\n");

      if (mesg.GetTimestamp() != FIT_UINT32_INVALID)
      {
         printf("   Timestamp: %d\n", mesg.GetTimestamp());
      }

      if(mesg.GetActivityType() != FIT_ACTIVITY_TYPE_INVALID)
      {
         printf("   Activity type: %d\n", mesg.GetActivityType());
      }

      switch(mesg.GetActivityType()) // The Cycling field is dynamic
      {
      case FIT_ACTIVITY_TYPE_WALKING:
      case FIT_ACTIVITY_TYPE_RUNNING: // Intentional fallthrough
         if(mesg.GetSteps() != FIT_UINT32_INVALID)
         {
            printf("   Steps: %d\n", mesg.GetSteps());
         }
         break;
      case FIT_ACTIVITY_TYPE_CYCLING:
      case FIT_ACTIVITY_TYPE_SWIMMING: // Intentional fallthrough
         if(mesg.GetStrokes() != (FIT_FLOAT32)(FIT_UINT32_INVALID/2) )
         {
            printf(   "Strokes: %d\n", mesg.GetStrokes());
         }
         break;
      default:
         if(mesg.GetCycles() != (FIT_FLOAT32)(FIT_UINT32_INVALID/2) )
         {
            printf(   "Cycles: %d\n", mesg.GetStrokes());
         }
         break;
      }
   }
};

int main(int argc, char* argv[])
{
   fit::Decode decode;
   fit::MesgBroadcaster mesgBroadcaster;
   Listener listener;
   std::fstream file;

   printf("FIT Decode Example Application\n");

   if (argc != 2)
   {
      printf("Usage: decode.exe <filename>\n");
      return -1;
   }

   file.open(argv[1], std::ios::in | std::ios::binary);

   if (!file.is_open())
   {
      printf("Error opening file %s\n", argv[1]);
      return -1;
   }

   if (!decode.CheckIntegrity(file))
   {
      printf("FIT file integrity failed.\nAttempting to decode...\n");
   }

   mesgBroadcaster.AddListener((fit::FileIdMesgListener &)listener);
   mesgBroadcaster.AddListener((fit::UserProfileMesgListener &)listener);
   mesgBroadcaster.AddListener((fit::MonitoringMesgListener &)listener);
   mesgBroadcaster.AddListener((fit::DeviceInfoMesgListener &)listener);
   //mesgBroadcaster.AddListener((fit::MesgListener &)listener);

   try
   {
      mesgBroadcaster.Run(file);
   }
   catch (const fit::RuntimeException& e)
   {
      printf("Exception decoding file: %s\n", e.what());
      return -1;
   }

   printf("Decoded FIT file %s.\n", argv[1]);

   return 0;
}

