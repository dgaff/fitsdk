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
// Copyright 2015 Dynastream Innovations Inc.
////////////////////////////////////////////////////////////////////////////////
// ****WARNING****  This file is auto-generated!  Do NOT edit this file.
// Profile Version = 16.00Release
// Tag = development-akw-16.00.00-0
////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading;

namespace Dynastream.Fit
{
   /// <summary>
   /// This class will decode a .fit file reading the file header and any definition or data messages. 
   /// </summary>
   public class Decode
   {
      #region Fields      
      private MesgDefinition[] localMesgDefs = new MesgDefinition[Fit.MaxLocalMesgs];            
      private Header fileHeader;      
      private uint timestamp = 0;
      private int lastTimeOffset = 0;
      #endregion

      #region Properties
      #endregion

      #region Constructors
      public Decode()
      {         
      }
      #endregion

      #region Methods
      public event MesgEventHandler MesgEvent;
      public event MesgDefinitionEventHandler MesgDefinitionEvent;

      /// <summary>
      /// Reads the file header to check if the file is FIT.
      /// Does not check CRC.
      /// Returns true if file is FIT.
      /// </summary>
      /// <param name="fitStream"> Seekable (file)stream to parse</param>      
      public bool IsFIT(Stream fitStream)
      {
         try
         {
            // Does the header contain the flag string ".FIT"?
            Header header = new Header(fitStream);
            return header.IsValid();      
         }
         // If the header is malformed the ctor could throw an exception
         catch (FitException)
         {
            return false;
         }         
      }

      /// <summary>
      /// Reads the FIT binary file header and crc to check compatibility and integrity.
      /// Also checks data reords size.  
      /// Returns true if file is ok (not corrupt).
      ///</summary>
      /// <param name="fitStream">Seekable (file)stream to parse.</param>     
      public bool CheckIntegrity(Stream fitStream)
      {
         bool isValid;
         
         try
         {
            // Is there a valid header?
            Header header = new Header(fitStream);
            isValid = header.IsValid();                           
            
            // Are there as many data bytes as the header claims?         
            isValid &= ((header.Size + header.DataSize + 2) == fitStream.Length);
     
            // Is the file CRC ok?                  
            byte[] data = new byte[fitStream.Length];
            fitStream.Position = 0;
            fitStream.Read(data, 0, data.Length);         
            isValid &= (CRC.Calc16(data, data.Length) == 0x0000);
                
            return isValid;
         }
         catch(FitException)
         {
            return false;
         }
      }
            
      /// <summary>
      /// Reads a FIT binary file. 
      /// </summary>
      /// <param name="fitStream">Seekable (file)stream to parse.</param>
      /// <returns>
      /// Returns true if reading finishes successfully.
      /// </returns>                       
      public bool Read(Stream fitStream)
      {
         bool readOK = true;                                   
       
         try
         {
            // Attempt to read header
            fileHeader = new Header(fitStream);
            readOK &= fileHeader.IsValid();

            if (!readOK)
            {
               throw new FitException("FIT decode error: File is not FIT format. Check file header data type.");
            }
            if ((fileHeader.ProtocolVersion & Fit.ProtocolVersionMajorMask) > (Fit.ProtocolMajorVersion << Fit.ProtocolVersionMajorShift))
            {
               // The decoder does not support decode accross protocol major revisions
               throw new FitException(String.Format("FIT decode error: Protocol Version {0}.X not supported by SDK Protocol Ver{1}.{2} ", (fileHeader.ProtocolVersion & Fit.ProtocolVersionMajorMask) >> Fit.ProtocolVersionMajorShift, Fit.ProtocolMajorVersion, Fit.ProtocolMinorVersion));
            }
           
            // Read data messages and definitions                               
            while (fitStream.Position < fitStream.Length - 2)
            {               
               DecodeNextMessage(fitStream);
            }
            // Is the file CRC ok?                  
            byte[] data = new byte[fitStream.Length];
            fitStream.Position = 0;
            fitStream.Read(data, 0, data.Length);
            readOK &= (CRC.Calc16(data, data.Length) == 0x0000);            
         }
         catch (EndOfStreamException e)
         {
            readOK = false;                        
            Debug.WriteLine("{0} caught and ignored. ", e.GetType().Name);
            throw new FitException("Decode:Read - Unexpected End of File", e);
         }         
         return readOK;
      }

      public void DecodeNextMessage(Stream fitStream)
      {
         BinaryReader br = new BinaryReader(fitStream);      
         byte nextByte = br.ReadByte();
         
         // Is it a compressed timestamp mesg?
         if ((nextByte & Fit.CompressedHeaderMask) == Fit.CompressedHeaderMask)
         {
            MemoryStream mesgBuffer = new MemoryStream();

            int timeOffset = nextByte & Fit.CompressedTimeMask;
            timestamp += (uint)((timeOffset - lastTimeOffset) & Fit.CompressedTimeMask);
            lastTimeOffset = timeOffset;
            Field timestampField = new Field(Profile.mesgs[Profile.RecordIndex].GetField("Timestamp"));
            timestampField.SetValue(timestamp);

            byte localMesgNum = (byte)((nextByte & Fit.CompressedLocalMesgNumMask) >> 5);
            mesgBuffer.WriteByte(localMesgNum);
            if (localMesgDefs[localMesgNum] == null)
            {
               throw new FitException("Decode:DecodeNextMessage - FIT decode error: Missing message definition for local message number " + localMesgNum + ".");
            }
            int fieldsSize = localMesgDefs[localMesgNum].GetMesgSize() - 1;
            try
            {            
               mesgBuffer.Write(br.ReadBytes(fieldsSize), 0, fieldsSize);
            }
            catch (IOException e)
            {
               throw new FitException("Decode:DecodeNextMessage - Compressed Data Message unexpected end of file.  Wanted " + fieldsSize + " bytes at stream position " + fitStream.Position, e);
            }         

            Mesg newMesg = new Mesg(mesgBuffer, localMesgDefs[localMesgNum]);
            newMesg.InsertField(0, timestampField);
            if (MesgEvent != null)
            {
               MesgEvent(this, new MesgEventArgs(newMesg));
            }
         }           
         // Is it a mesg def?
         else if ((nextByte & Fit.HeaderTypeMask) == Fit.MesgDefinitionMask)
         {
            MemoryStream mesgDefBuffer = new MemoryStream();

            // Figure out number of fields (length) of our defn and build buffer
            mesgDefBuffer.WriteByte(nextByte);
            mesgDefBuffer.Write(br.ReadBytes(4), 0, 4);               
            byte numfields = br.ReadByte();
            mesgDefBuffer.WriteByte(numfields);
            try
            {           
               mesgDefBuffer.Write(br.ReadBytes(numfields * 3), 0, numfields * 3);
            }
            catch (IOException e)
            {
               throw new FitException("Decode:DecodeNextMessage - Defn Message unexpected end of file.  Wanted " + (numfields * 3) + " bytes at stream position " + fitStream.Position, e);
            }         

            MesgDefinition newMesgDef = new MesgDefinition(mesgDefBuffer);                  
            localMesgDefs[newMesgDef.LocalMesgNum] = newMesgDef;                  
            if (MesgDefinitionEvent != null)
            {
               MesgDefinitionEvent(this, new MesgDefinitionEventArgs(newMesgDef));                     
            }
         }         
         // Is it a data mesg?         
         else if ((nextByte & Fit.HeaderTypeMask) == Fit.MesgHeaderMask)
         {
            MemoryStream mesgBuffer = new MemoryStream();

            byte localMesgNum = (byte)(nextByte & Fit.LocalMesgNumMask);
            mesgBuffer.WriteByte(localMesgNum);                  
            if (localMesgDefs[localMesgNum] == null)
            {
               throw new FitException("Decode:DecodeNextMessage - FIT decode error: Missing message definition for local message number " + localMesgNum + ".");
            }
            int fieldsSize = localMesgDefs[localMesgNum].GetMesgSize() - 1;
            try
            {
               mesgBuffer.Write(br.ReadBytes(fieldsSize), 0, fieldsSize);
            }
            catch (Exception e)
            {
               throw new FitException("Decode:DecodeNextMessage - Data Message unexpected end of file.  Wanted " + fieldsSize + " bytes at stream position " + fitStream.Position, e);
            }         

            Mesg newMesg = new Mesg(mesgBuffer, localMesgDefs[localMesgNum]);

            // If the new message contains a timestamp field, record the value to use as
            // a reference for compressed timestamp headers
            Field timestampField = newMesg.GetField("Timestamp");
            if (timestampField != null)
            {
               timestamp = (uint)timestampField.GetValue();
               lastTimeOffset = (int)timestamp & Fit.CompressedTimeMask;
            }
            // Now that the entire message is decoded we can evaluate subfields and expand any components
            newMesg.ExpandComponents();            

            if (MesgEvent != null)
            {
               MesgEvent(this, new MesgEventArgs(newMesg));
            }
         }
         else
         {
            throw new FitException("Decode:Read - FIT decode error: Unexpected Record Header Byte 0x" + nextByte.ToString("X"));
         }
      }      
      #endregion
   } // class
} // namespace
