#region Copyright
////////////////////////////////////////////////////////////////////////////////
// The following FIT Protocol software provided may be used with FIT protocol
// devices only and remains the copyrighted property of Dynastream Innovations Inc.
// The software is being provided on an "as-is" basis and as an accommodation,
// and therefore all warranties, representations, or guarantees of any kind
// (whether express, implied or statutory) including, without limitation,
// warranties of merchantability, non-infringement, or fitness for a particular
// purpose, are specifically disclaimed.
//
// Copyright 2012 Dynastream Innovations Inc.
////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Dynastream.Fit;

namespace EncodeDemo
{
   class Program
   {
      static void Main(string[] args)
      {
         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();

         // Encode both of our example files
         EncodeSettingsFile();
         EncodeMonitoringFile();

         stopwatch.Stop();
         Console.WriteLine("Time elapsed: {0:0.#}s", stopwatch.Elapsed.TotalSeconds);
      }

      /// <summary>
      /// Demonstrate the encoding of a 'Settings File' by writing a 'Settings File' containing a 'User Profile' Message.
      /// This example is simpler than the 'Monitoring File' example.
      /// </summary>
      static void EncodeSettingsFile()
      {
         // Generate some FIT messages
         FileIdMesg fileIdMesg = new FileIdMesg(); // Every FIT file MUST contain a 'File ID' message as the first message
         fileIdMesg.SetType(Dynastream.Fit.File.Settings);
         fileIdMesg.SetManufacturer(Manufacturer.Dynastream);  // Types defined in the profile are available
         fileIdMesg.SetProduct(1000);
         fileIdMesg.SetSerialNumber(12345);

         UserProfileMesg myUserProfile = new UserProfileMesg();
         myUserProfile.SetGender(Gender.Female);
         float myWeight = 63.1F;
         myUserProfile.SetWeight(myWeight);
         myUserProfile.SetAge(99);
         myUserProfile.SetFriendlyName(Encoding.UTF8.GetBytes("TestUser"));

         FileStream fitDest = new FileStream("ExampleSettings.fit", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

         // Create file encode object
         Encode encodeDemo = new Encode();

         // Write our header
         encodeDemo.Open(fitDest);

         // Encode each message, a definition message is automatically generated and output if necessary
         encodeDemo.Write(fileIdMesg);
         encodeDemo.Write(myUserProfile);

         // Update header datasize and file CRC
         encodeDemo.Close();
         fitDest.Close();

         Console.WriteLine("Encoded FIT file ExampleSettings.fit");
         return;
      }

      /// <summary>
      /// Demonstrates encoding a 'MonitoringB File' of a made up device which counts steps and reports the battery status of the device.
      /// </summary>
      static void EncodeMonitoringFile()
      {
         System.DateTime systemStartTime = System.DateTime.Now;
         System.DateTime systemTimeNow = systemStartTime;

         FileStream fitDest = new FileStream("ExampleMonitoringFile.fit", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);

         // Create file encode object
         Encode encodeDemo = new Encode();

         // Write our header
         encodeDemo.Open(fitDest);

         // Generate some FIT messages
         FileIdMesg fileIdMesg = new FileIdMesg(); // Every FIT file MUST contain a 'File ID' message as the first message
         fileIdMesg.SetType(Dynastream.Fit.File.MonitoringB); // See the 'FIT FIle Types Description' document for more information about this file type.
         fileIdMesg.SetManufacturer(Manufacturer.Dynastream);
         fileIdMesg.SetProduct(1001);
         fileIdMesg.SetSerialNumber(54321);
         fileIdMesg.SetTimeCreated(new Dynastream.Fit.DateTime(systemTimeNow));
         fileIdMesg.SetNumber(0);
         encodeDemo.Write(fileIdMesg); // Write the 'File ID Message'

         DeviceInfoMesg deviceInfoMesg = new DeviceInfoMesg();
         deviceInfoMesg.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow));
         deviceInfoMesg.SetBatteryStatus(Dynastream.Fit.BatteryStatus.Good);
         encodeDemo.Write(deviceInfoMesg);

         MonitoringMesg monitoringMesg = new MonitoringMesg();

         // By default, each time a new message is written the Local Message Type 0 will be redefined to match the new message.
         // In this case,to avoid having a definition message each time there is a DeviceInfoMesg, we can manually set the Local Message Type of the MonitoringMessage to '1'.
         // By doing this we avoid an additional 7 definition messages in our FIT file.
         monitoringMesg.LocalNum = 1;

         // Simulate some data
         Random numberOfCycles = new Random(); // Fake a number of cycles
         for (int i = 0; i < 4; i++) // Each of these loops represent a quarter of a day
         {
            for (int j = 0; j < 6; j++) // Each of these loops represent 1 hour
            {
               monitoringMesg.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow));
               monitoringMesg.SetActivityType(Dynastream.Fit.ActivityType.Walking); // Setting this to walking will cause Cycles to be interpretted as steps.
               monitoringMesg.SetCycles(monitoringMesg.GetCycles() + numberOfCycles.Next(0, 1000)); // Cycles are accumulated (i.e. must be increasing)
               encodeDemo.Write(monitoringMesg);
               systemTimeNow = systemTimeNow.AddHours(1); // Add an hour to our contrieved timestamp
            }

            deviceInfoMesg.SetTimestamp(new Dynastream.Fit.DateTime(systemTimeNow));
            deviceInfoMesg.SetBatteryStatus(Dynastream.Fit.BatteryStatus.Good); // Report the battery status every quarter day
            encodeDemo.Write(deviceInfoMesg);
         }

         // Update header datasize and file CRC
         encodeDemo.Close();
         fitDest.Close();

         Console.WriteLine("Encoded FIT file ExampleMonitoringFile.fit");
      }
   }
}
