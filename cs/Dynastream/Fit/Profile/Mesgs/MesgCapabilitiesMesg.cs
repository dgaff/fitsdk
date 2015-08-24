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


namespace Dynastream.Fit
{
   /// <summary>
   /// Implements the MesgCapabilities profile message.
   /// </summary>  
   public class MesgCapabilitiesMesg : Mesg 
   {    
      #region Fields            
      static class CountSubfield
      {
         public static ushort NumPerFile = 0;
         public static ushort MaxPerFile = 1;
         public static ushort MaxPerFileType = 2;
         public static ushort Subfields = 3;
         public static ushort Active = Fit.SubfieldIndexActiveSubfield;
         public static ushort MainField = Fit.SubfieldIndexMainField;
      }     
      #endregion

      #region Constructors                 
      public MesgCapabilitiesMesg() : base(Profile.mesgs[Profile.MesgCapabilitiesIndex])               
      {                 
      }
      
      public MesgCapabilitiesMesg(Mesg mesg) : base(mesg)
      {
      }
      #endregion // Constructors

      #region Methods    
      ///<summary>      
      /// Retrieves the MessageIndex field</summary>
      /// <returns>Returns nullable ushort representing the MessageIndex field</returns>      
      public ushort? GetMessageIndex()   
      {                
         return (ushort?)GetFieldValue(254, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set MessageIndex field</summary>
      /// <param name="messageIndex_">Nullable field value to be set</param>      
      public void SetMessageIndex(ushort? messageIndex_) 
      {  
         SetFieldValue(254, 0, messageIndex_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the File field</summary>
      /// <returns>Returns nullable File enum representing the File field</returns>      
      public File? GetFile()   
      { 
         object obj = GetFieldValue(0, 0, Fit.SubfieldIndexMainField);
         File? value = obj == null ? (File?)null : (File)obj;
         return value;                     
      }

      /// <summary>        
      /// Set File field</summary>
      /// <param name="file_">Nullable field value to be set</param>      
      public void SetFile(File? file_) 
      {  
         SetFieldValue(0, 0, file_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the MesgNum field</summary>
      /// <returns>Returns nullable ushort representing the MesgNum field</returns>      
      public ushort? GetMesgNum()   
      {                
         return (ushort?)GetFieldValue(1, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set MesgNum field</summary>
      /// <param name="mesgNum_">Nullable field value to be set</param>      
      public void SetMesgNum(ushort? mesgNum_) 
      {  
         SetFieldValue(1, 0, mesgNum_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the CountType field</summary>
      /// <returns>Returns nullable MesgCount enum representing the CountType field</returns>      
      public MesgCount? GetCountType()   
      { 
         object obj = GetFieldValue(2, 0, Fit.SubfieldIndexMainField);
         MesgCount? value = obj == null ? (MesgCount?)null : (MesgCount)obj;
         return value;                     
      }

      /// <summary>        
      /// Set CountType field</summary>
      /// <param name="countType_">Nullable field value to be set</param>      
      public void SetCountType(MesgCount? countType_) 
      {  
         SetFieldValue(2, 0, countType_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the Count field</summary>
      /// <returns>Returns nullable ushort representing the Count field</returns>      
      public ushort? GetCount()   
      {                
         return (ushort?)GetFieldValue(3, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set Count field</summary>
      /// <param name="count_">Nullable field value to be set</param>      
      public void SetCount(ushort? count_) 
      {  
         SetFieldValue(3, 0, count_, Fit.SubfieldIndexMainField);
      }
      

      /// <summary>       
      /// Retrieves the NumPerFile subfield</summary>      
      /// <returns>Nullable ushort representing the NumPerFile subfield</returns>      
      public ushort? GetNumPerFile() 
      {                               
         return (ushort?)GetFieldValue(3, 0, CountSubfield.NumPerFile);   
      }

      /// <summary> 
      /// 
      /// Set NumPerFile subfield</summary>
      /// <param name="numPerFile">Subfield value to be set</param>      
      public void SetNumPerFile(ushort? numPerFile) 
      {  
         SetFieldValue(3, 0, numPerFile, CountSubfield.NumPerFile);
      }

      /// <summary>       
      /// Retrieves the MaxPerFile subfield</summary>      
      /// <returns>Nullable ushort representing the MaxPerFile subfield</returns>      
      public ushort? GetMaxPerFile() 
      {                               
         return (ushort?)GetFieldValue(3, 0, CountSubfield.MaxPerFile);   
      }

      /// <summary> 
      /// 
      /// Set MaxPerFile subfield</summary>
      /// <param name="maxPerFile">Subfield value to be set</param>      
      public void SetMaxPerFile(ushort? maxPerFile) 
      {  
         SetFieldValue(3, 0, maxPerFile, CountSubfield.MaxPerFile);
      }

      /// <summary>       
      /// Retrieves the MaxPerFileType subfield</summary>      
      /// <returns>Nullable ushort representing the MaxPerFileType subfield</returns>      
      public ushort? GetMaxPerFileType() 
      {                               
         return (ushort?)GetFieldValue(3, 0, CountSubfield.MaxPerFileType);   
      }

      /// <summary> 
      /// 
      /// Set MaxPerFileType subfield</summary>
      /// <param name="maxPerFileType">Subfield value to be set</param>      
      public void SetMaxPerFileType(ushort? maxPerFileType) 
      {  
         SetFieldValue(3, 0, maxPerFileType, CountSubfield.MaxPerFileType);
      }                  
      #endregion // Methods
   } // Class
} // namespace
