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


namespace DecodeDemo
{
   class Program
   {
      static Dictionary<ushort, int> mesgCounts = new Dictionary<ushort, int>();
      static FileStream fitSource;

      static void Main(string[] args)
      {
         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();

         Console.WriteLine("FIT Decode Example Application");

         if (args.Length != 1)
         {
            Console.WriteLine("Usage: decode.exe <filename>");
            return;
         }

         // Attempt to open .FIT file
         fitSource = new FileStream(args[0], FileMode.Open);
         Console.WriteLine("Opening {0}", args[0]);

         Decode decodeDemo = new Decode();
         MesgBroadcaster mesgBroadcaster = new MesgBroadcaster();

         // Connect the Broadcaster to our event (message) source (in this case the Decoder)
         decodeDemo.MesgEvent += mesgBroadcaster.OnMesg;
         decodeDemo.MesgDefinitionEvent += mesgBroadcaster.OnMesgDefinition;

         // Subscribe to message events of interest by connecting to the Broadcaster
         mesgBroadcaster.MesgEvent += new MesgEventHandler(OnMesg);
         mesgBroadcaster.MesgDefinitionEvent += new MesgDefinitionEventHandler(OnMesgDefn);

         mesgBroadcaster.FileIdMesgEvent += new MesgEventHandler(OnFileIDMesg);
         mesgBroadcaster.UserProfileMesgEvent += new MesgEventHandler(OnUserProfileMesg);

         bool status = decodeDemo.IsFIT(fitSource);
         status &= decodeDemo.CheckIntegrity(fitSource);
         // Process the file
         if (status == true)
         {
            Console.WriteLine("Decoding...");
            decodeDemo.Read(fitSource);
            Console.WriteLine("Decoded FIT file {0}", args[0]);
         }
         else
         {
            try
            {
               Console.WriteLine("Integrity Check Failed {0}", args[0]);
               Console.WriteLine("Attempting to decode...");
               decodeDemo.Read(fitSource);
            }
            catch (FitException ex)
            {
               Console.WriteLine("DecodeDemo caught FitException: " + ex.Message);
            }
         }
         fitSource.Close();

         Console.WriteLine("");
         Console.WriteLine("Summary:");
         int totalMesgs = 0;
         foreach (KeyValuePair<ushort, int> pair in mesgCounts)
         {
            Console.WriteLine("MesgID {0,3} Count {1}", pair.Key, pair.Value);
            totalMesgs += pair.Value;
         }

         Console.WriteLine("{0} Message Types {1} Total Messages", mesgCounts.Count, totalMesgs);

         stopwatch.Stop();
         Console.WriteLine("");
         Console.WriteLine("Time elapsed: {0:0.#}s", stopwatch.Elapsed.TotalSeconds);
         Console.ReadKey();
         return;
      }

      #region Message Handlers
      // Client implements their handlers of interest and subscribes to MesgBroadcaster events
      static void OnMesgDefn(object sender, MesgDefinitionEventArgs e)
      {
         Console.WriteLine("OnMesgDef: Received Defn for local message #{0}, global num {1}", e.mesgDef.LocalMesgNum, e.mesgDef.GlobalMesgNum);
         Console.WriteLine("\tIt has {0} fields and is {1} bytes long", e.mesgDef.NumFields, e.mesgDef.GetMesgSize());
      }

      static void OnMesg(object sender, MesgEventArgs e)
      {
         Console.WriteLine("OnMesg: Received Mesg with global ID#{0}, its name is {1}", e.mesg.Num, e.mesg.Name);

         for (byte i = 0; i < e.mesg.GetNumFields(); i++)
         {
            for (int j = 0; j < e.mesg.fields[i].GetNumValues(); j++)
            {
               Console.WriteLine("\tField{0} Index{1} (\"{2}\" Field#{4}) Value: {3} (raw value {5})", i, j, e.mesg.fields[i].GetName(), e.mesg.fields[i].GetValue(j), e.mesg.fields[i].Num, e.mesg.fields[i].GetRawValue(j));
            }
         }

         if (mesgCounts.ContainsKey(e.mesg.Num) == true)
         {
            mesgCounts[e.mesg.Num]++;
         }
         else
         {
            mesgCounts.Add(e.mesg.Num, 1);
         }
      }

      static void OnFileIDMesg(object sender, MesgEventArgs e)
      {
         Console.WriteLine("FileIdHandler: Received {1} Mesg with global ID#{0}", e.mesg.Num, e.mesg.Name);
         FileIdMesg myFileId = (FileIdMesg)e.mesg;
         try
         {
            Console.WriteLine("\tType: {0}", myFileId.GetType());
            Console.WriteLine("\tManufacturer: {0}", myFileId.GetManufacturer());
            Console.WriteLine("\tProduct: {0}", myFileId.GetProduct());
            Console.WriteLine("\tSerialNumber {0}", myFileId.GetSerialNumber());
            Console.WriteLine("\tNumber {0}", myFileId.GetNumber());
            Console.WriteLine("\tTimeCreated {0}", myFileId.GetTimeCreated());
            Dynastream.Fit.DateTime dtTime = new Dynastream.Fit.DateTime(myFileId.GetTimeCreated().GetTimeStamp());

         }
         catch (FitException exception)
         {
            Console.WriteLine("\tOnFileIDMesg Error {0}", exception.Message);
            Console.WriteLine("\t{0}", exception.InnerException);
         }
      }
      static void OnUserProfileMesg(object sender, MesgEventArgs e)
      {
         Console.WriteLine("UserProfileHandler: Received {1} Mesg, it has global ID#{0}", e.mesg.Num, e.mesg.Name);
         UserProfileMesg myUserProfile = (UserProfileMesg)e.mesg;
         try
         {
            Console.WriteLine("\tFriendlyName \"{0}\"", Encoding.UTF8.GetString(myUserProfile.GetFriendlyName()));
            Console.WriteLine("\tGender {0}", myUserProfile.GetGender().ToString());
            Console.WriteLine("\tAge {0}", myUserProfile.GetAge());
            Console.WriteLine("\tWeight  {0}", myUserProfile.GetWeight());
         }
         catch (FitException exception)
         {
            Console.WriteLine("\tOnUserProfileMesg Error {0}", exception.Message);
            Console.WriteLine("\t{0}", exception.InnerException);
         }
      }

      static void OnDeviceInfoMessage(object sender, MesgEventArgs e)
      {
         Console.WriteLine("DeviceInfoHandler: Received {1} Mesg, it has global ID#{0}", e.mesg.Num, e.mesg.Name);
         DeviceInfoMesg myDeviceInfoMessage = (DeviceInfoMesg)e.mesg;
         try
         {
            Console.WriteLine("\tTimestamp  {0}", myDeviceInfoMessage.GetTimestamp());
            Console.WriteLine("\tBattery Status{0}", myDeviceInfoMessage.GetBatteryStatus());
         }
         catch (FitException exception)
         {
            Console.WriteLine("\tOnDeviceInfoMesg Error {0}", exception.Message);
            Console.WriteLine("\t{0}", exception.InnerException);
         }
      }

      static void OnMonitoringMessage(object sender, MesgEventArgs e)
      {
         Console.WriteLine("MonitoringHandler: Received {1} Mesg, it has global ID#{0}", e.mesg.Num, e.mesg.Name);
         MonitoringMesg myMonitoringMessage = (MonitoringMesg)e.mesg;
         try
         {
            Console.WriteLine("\tTimestamp  {0}", myMonitoringMessage.GetTimestamp());
            Console.WriteLine("\tActivityType {0}", myMonitoringMessage.GetActivityType());
            switch (myMonitoringMessage.GetActivityType()) // Cycles is a dynamic field
            {
               case ActivityType.Walking:
               case ActivityType.Running:
                  Console.WriteLine("\tSteps {0}", myMonitoringMessage.GetSteps());
                  break;
               case ActivityType.Cycling:
               case ActivityType.Swimming:
                  Console.WriteLine("\tStrokes {0}", myMonitoringMessage.GetStrokes());
                  break;
               default:
                  Console.WriteLine("\tCycles {0}", myMonitoringMessage.GetCycles());
                  break;
            }
         }
         catch (FitException exception)
         {
            Console.WriteLine("\tOnDeviceInfoMesg Error {0}", exception.Message);
            Console.WriteLine("\t{0}", exception.InnerException);
         }
      }
      #endregion
   }
}
