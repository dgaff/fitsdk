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

public enum ActivityLevel {
   LOW((short)0),
   MEDIUM((short)1),
   HIGH((short)2),
   INVALID((short)255);


   protected short value;




   private ActivityLevel(short value) {
     this.value = value;
   }

   public static ActivityLevel getByValue(final Short value) {
      for (final ActivityLevel type : ActivityLevel.values()) {
         if (value == type.value)
            return type;
      }

      return ActivityLevel.INVALID;
   }

   public short getValue() {
      return value;
   }


}
