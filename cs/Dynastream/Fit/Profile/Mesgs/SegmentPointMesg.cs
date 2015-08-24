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
   /// Implements the SegmentPoint profile message.
   /// </summary>  
   public class SegmentPointMesg : Mesg 
   {    
      #region Fields     
      #endregion

      #region Constructors                 
      public SegmentPointMesg() : base(Profile.mesgs[Profile.SegmentPointIndex])               
      {                 
      }
      
      public SegmentPointMesg(Mesg mesg) : base(mesg)
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
      /// Retrieves the PositionLat field
      /// Units: semicircles</summary>
      /// <returns>Returns nullable int representing the PositionLat field</returns>      
      public int? GetPositionLat()   
      {                
         return (int?)GetFieldValue(1, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set PositionLat field
      /// Units: semicircles</summary>
      /// <param name="positionLat_">Nullable field value to be set</param>      
      public void SetPositionLat(int? positionLat_) 
      {  
         SetFieldValue(1, 0, positionLat_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the PositionLong field
      /// Units: semicircles</summary>
      /// <returns>Returns nullable int representing the PositionLong field</returns>      
      public int? GetPositionLong()   
      {                
         return (int?)GetFieldValue(2, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set PositionLong field
      /// Units: semicircles</summary>
      /// <param name="positionLong_">Nullable field value to be set</param>      
      public void SetPositionLong(int? positionLong_) 
      {  
         SetFieldValue(2, 0, positionLong_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the Distance field
      /// Units: m
      /// Comment: Accumulated distance along the segment at the described point</summary>
      /// <returns>Returns nullable float representing the Distance field</returns>      
      public float? GetDistance()   
      {                
         return (float?)GetFieldValue(3, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set Distance field
      /// Units: m
      /// Comment: Accumulated distance along the segment at the described point</summary>
      /// <param name="distance_">Nullable field value to be set</param>      
      public void SetDistance(float? distance_) 
      {  
         SetFieldValue(3, 0, distance_, Fit.SubfieldIndexMainField);
      }
          
      ///<summary>      
      /// Retrieves the Altitude field
      /// Units: m
      /// Comment: Accumulated altitude along the segment at the described point</summary>
      /// <returns>Returns nullable float representing the Altitude field</returns>      
      public float? GetAltitude()   
      {                
         return (float?)GetFieldValue(4, 0, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set Altitude field
      /// Units: m
      /// Comment: Accumulated altitude along the segment at the described point</summary>
      /// <param name="altitude_">Nullable field value to be set</param>      
      public void SetAltitude(float? altitude_) 
      {  
         SetFieldValue(4, 0, altitude_, Fit.SubfieldIndexMainField);
      }
          
      
      /// <summary>
      /// 
      /// </summary>  
      /// <returns>returns number of elements in field LeaderTime</returns>      
      public int GetNumLeaderTime() 
      {
         return GetNumFieldValues(5, Fit.SubfieldIndexMainField);
      }

      ///<summary>      
      /// Retrieves the LeaderTime field
      /// Units: s
      /// Comment: Accumualted time each leader board member required to reach the described point. This value is zero for all leader board members at the starting point of the segment. </summary>
      /// <param name="index">0 based index of LeaderTime element to retrieve</param>
      /// <returns>Returns nullable float representing the LeaderTime field</returns>      
      public float? GetLeaderTime(int index)   
      {                
         return (float?)GetFieldValue(5, index, Fit.SubfieldIndexMainField);                     
      }

      /// <summary>        
      /// Set LeaderTime field
      /// Units: s
      /// Comment: Accumualted time each leader board member required to reach the described point. This value is zero for all leader board members at the starting point of the segment. </summary>      
      /// <param name="index">0 based index of leader_time</param>
      /// <param name="leaderTime_">Nullable field value to be set</param>      
      public void SetLeaderTime(int index, float? leaderTime_) 
      {  
         SetFieldValue(5, index, leaderTime_, Fit.SubfieldIndexMainField);
      }
                        
      #endregion // Methods
   } // Class
} // namespace
