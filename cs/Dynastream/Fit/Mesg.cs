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

using Dynastream.Utility;

namespace Dynastream.Fit
{
   /// <summary>
   /// 
   /// </summary>  
   public class Mesg
   {
      #region Fields                        
      protected byte localNum = 0;      
      protected uint systemTimeOffset = 0;      
      public List<Field> fields = new List<Field>();
      #endregion

      #region Properties
      public string Name { get; set; }
      public ushort Num { get; set; }
      public byte LocalNum
      {
         get
         {
            return localNum;
         }
         set
         {
            if (value > Fit.LocalMesgNumMask)
            {               
               throw new FitException("Mesg:LocalNum - Invalid Local message number " + value + ". Local message number must be < " + Fit.LocalMesgNumMask);
            }
            else
            {
               localNum = value;
            }
         }
      }  
      #endregion

      #region Constructors
      public Mesg(Mesg mesg)
      {
         if (mesg == null)
         {
            this.Name = "unknown";
            this.Num = (ushort)MesgNum.Invalid;                        
            return;
         }
         this.Name = mesg.Name;
         this.Num = mesg.Num;
         this.LocalNum = mesg.LocalNum;
         this.systemTimeOffset = mesg.systemTimeOffset;                  
         foreach (Field field in mesg.fields)
         {
            if (field.GetNumValues() > 0) 
            {
               this.fields.Add(new Field(field));
            }
         }
      }
      
      public Mesg(string name, ushort num)
      {         
         this.Name = name;
         this.Num = num;         
      }

      internal Mesg(ushort mesgNum) : this(Profile.GetMesg(mesgNum))
      {         
      }
      
      public Mesg(Stream fitStream, MesgDefinition defnMesg) : this(defnMesg.GlobalMesgNum)
      {                
         Read(fitStream, defnMesg);         
      }
      #endregion

      #region Methods
      public void Read(Stream inStream, MesgDefinition defnMesg)
      {
         inStream.Position = 1;
         EndianBinaryReader mesgReader = new EndianBinaryReader(inStream, defnMesg.IsBigEndian);
         
         LocalNum = defnMesg.LocalMesgNum;
                 
         foreach (FieldDefinition fieldDef in defnMesg.GetFields())
         {
            // It's possible the field type found in the field definition may
            // not agree with the type defined in the profile.  The profile 
            // type will be preferred for decode.  
            Field field = GetField(fieldDef.Num);         
            if (field == null)
            {
               // We normally won't have fields attached to our skeleton message, 
               // as we add values we need to add the fields too based on the mesg,field
               // combo in the profile.  Must derive from the profile so the scale etc
               // is correct            
               field = new Field(Profile.GetMesg(this.Num).GetField(fieldDef.Num));
               if (field.Num == Fit.FieldNumInvalid)
               {  
                  // If there was no info in the profile the FieldNum will get set to invalid
                  // so preserve the unknown fields info while we know it
                  field.Num = fieldDef.Num;
                  field.Type = fieldDef.Type;
               }
               SetField(field);            
            }
                        
            object value;

            // strings may be an array and are of variable length
            if ((field.Type & Fit.BaseTypeNumMask) == Fit.String)
            {     
               List<byte> utf8Bytes = new List<byte>();
               byte b = new byte();

               for (int i=0; i<fieldDef.Size; i++)
               {
                  b = mesgReader.ReadByte();
                  if (b == 0x00)
                  {
                     field.AddValue(utf8Bytes.ToArray());
                     utf8Bytes.Clear();
                  }
                  else
                  {
                     utf8Bytes.Add(b);
                  }
               }
               if (utf8Bytes.Count != 0)
               {
                  field.AddValue(utf8Bytes.ToArray());
                  utf8Bytes.Clear();
               }               
            }
            else
            {
               int numElements = (int)fieldDef.Size / Fit.BaseType[field.Type & Fit.BaseTypeNumMask].size;               
               for (int i=0; i < numElements; i++)
               {               
                  switch (field.Type & Fit.BaseTypeNumMask)
                  {
                     case Fit.Enum:
                     case Fit.Byte:
                     case Fit.UInt8:
                     case Fit.UInt8z:
                        value = mesgReader.ReadByte();
                        break;

                     case Fit.SInt8:
                        value = mesgReader.ReadSByte();
                        break;

                     case Fit.SInt16:
                        value = mesgReader.ReadInt16();
                        break;

                     case Fit.UInt16:
                     case Fit.UInt16z:
                        value = mesgReader.ReadUInt16();
                        break;

                     case Fit.SInt32:
                        value = mesgReader.ReadInt32();
                        break;

                     case Fit.UInt32:
                     case Fit.UInt32z:
                        value = mesgReader.ReadUInt32();
                        break;

                     case Fit.Float32:
                        value = mesgReader.ReadSingle();
                        break;

                     case Fit.Float64:
                        value = mesgReader.ReadDouble();
                        break;               

                     default:
                        value = mesgReader.ReadBytes(fieldDef.Size);
                        break;
                  }                        
                  field.SetRawValue(i, value);
               }
            }            
         }
      }

      public void Write(Stream outStream)
      {         
         Write(outStream, null);
      }

      public void Write(Stream outStream, MesgDefinition mesgDef) 
      {
         if (mesgDef == null)
         {
            mesgDef = new MesgDefinition(this);
         }

         EndianBinaryWriter bw = new EndianBinaryWriter(outStream, mesgDef.IsBigEndian);        
         bw.Write(LocalNum);

         foreach (FieldDefinition fieldDef in mesgDef.GetFields())
         {
            Field field = GetField(fieldDef.Num);
            if (field == null)
            {
               field = Profile.GetField(this.Num, fieldDef.Num);
               fields.Add(field);
            }                                    
            // The field could be blank, correctly formed or partially filled              
            while (field.GetSize() < fieldDef.Size)
            {
               field.AddValue(Fit.BaseType[fieldDef.Type & Fit.BaseTypeNumMask].invalidValue);               
            }
            
            for (int i=0; i<field.GetNumValues(); i++)
            {
               object value = field.GetRawValue(i);
               if (value == null)
               {
                  value = Fit.BaseType[fieldDef.Type & Fit.BaseTypeNumMask].invalidValue;                                    
               }
               switch (fieldDef.Type & Fit.BaseTypeNumMask)
               {
                  case Fit.Enum:
                  case Fit.Byte:
                  case Fit.UInt8:
                  case Fit.UInt8z:
                     bw.Write((byte)value);
                     break;

                  case Fit.SInt8:
                     bw.Write((sbyte)value);
                     break;

                  case Fit.SInt16:
                     bw.Write((short)value);
                     break;

                  case Fit.UInt16:
                  case Fit.UInt16z:
                     bw.Write((ushort)value);
                     break;

                  case Fit.SInt32:
                     bw.Write((int)value);
                     break;

                  case Fit.UInt32:
                  case Fit.UInt32z:
                     bw.Write((uint)value);
                     break;

                  case Fit.Float32:
                     bw.Write((float)value);
                     break;

                  case Fit.Float64:
                     bw.Write((double)value);
                     break;

                  case Fit.String:                          
                     bw.Write((byte[])value);
                     // Write the null terminator
                     bw.Write((byte)0x00);
                     break;

                  default:                     
                     break;
               }               
            }
         }
      }

      #region FieldList Manipulation Functions
      public bool HasField(byte fieldNum)
      {
         foreach (Field field in fields)
         {
            if (field.Num == fieldNum)
            {
               return true;
            }
         }
         return false;         
      }
      
      /// <summary>
      /// Replace an existing field, otherwise add a reference to fields list
      /// </summary>
      /// <param name="field">Caller allocated field</param>
      public void SetField(Field field)
      {
         for (int i = 0; i < fields.Count; i++)
         {
            if (fields[i].Num == field.Num)
            {      
               fields[i] = field;
               return;
            }
         }
         fields.Add(field);
      }

      /// <summary>
      /// Insert a field at the desired index.  If the field already exists in the mesg it is first removed.
      /// </summary>
      /// <param name="index">Index to insert the field, if index is out of range, the field is added to the end of the list</param>
      /// <param name="field">Caller allocated field</param>
      public void InsertField(int index, Field field)
      {
         // if message already contains this field, remove it
         for (int i = 0; i < fields.Count; i++)
         {
            if (fields[i].Num == field.Num)
            {      
               fields.RemoveAt(i);
            }
         }
         // if the index is out of range, add to the end 
         if (index < 0 || index > fields.Count)
         {
            fields.Add(field);
         }
         // insert the new field at desired index 
         else
         {            
            fields.Insert(index, field);
         }                  
      }

      public void SetFields(Mesg mesg)
      {
         if (mesg.Num != Num)
         {
            return;
         }      
         foreach (Field field in mesg.fields)
         {            
            SetField(new Field(field));
         }
      }

      public int GetNumFields()
      {
         return fields.Count;
      }

      public Field GetField(byte fieldNum)
      {
         foreach (Field field in fields) 
         {
            if (field.Num == fieldNum)
            {
               return field;
            }
         }
         return null;
      }

      public Field GetField(string fieldName)
      {
         return GetField(fieldName, true);         
      }
      
      public Field GetField(string fieldName, bool checkMesgSupportForSubFields)
      {
         foreach (Field field in fields)
         {
            if (field.Name == fieldName)
            {
               return field;
            }
            foreach (Subfield subfield in field.subfields)
            {             
               if ((subfield.Name == fieldName) && (!checkMesgSupportForSubFields || (subfield.CanMesgSupport(this))))
               {
                  return field;
               }
            }
         }
         return null;         
      }
            
      public ushort GetActiveSubFieldIndex(byte fieldNum) 
      {
         Field testField = new Field(this.GetField(fieldNum));
   
         if (testField == null)
         {
            return Fit.SubfieldIndexMainField;
         }

         for (ushort i = 0; i < testField.subfields.Count; i++)
         {         
            if (testField.subfields[i].CanMesgSupport(this))
            {
               return i;
            }
         }
         return Fit.SubfieldIndexMainField;         
      }  

      public string GetActiveSubFieldName(byte fieldNum) 
      {
         Field testField = new Field(this.GetField(fieldNum));

         if (testField == null)
         {
            return Fit.SubfieldNameMainField;
         }

         foreach (Subfield subfield in testField.subfields)
         {
            if (subfield.CanMesgSupport(this))
            {
               return subfield.Name;
            }
         }
         return Fit.SubfieldNameMainField;         
      }          
      #endregion

      public int GetNumFieldValues(byte fieldNum)
      {
         Field field = GetField(fieldNum);

         if (field != null)
         {
            return field.GetNumValues();
         }
         return 0;
      }

      public int GetNumFieldValues(String fieldName) 
      {
         Field field = GetField(fieldName);

         if (field != null)
         {
            return field.GetNumValues();
         }
         return 0;
      }
      
      public int GetNumFieldValues(byte fieldNum, ushort subfieldIndex)
      {
         Field field = GetField(fieldNum);

         if (field == null)
         {
            return 0;
         }

         if (subfieldIndex == Fit.SubfieldIndexActiveSubfield)
         {
            return field.GetNumValues();
         }
         
         Subfield subfield = field.GetSubfield(subfieldIndex);
         if ((subfield == null) || (subfield.CanMesgSupport(this)))
         {
            return field.GetNumValues();
         }
         else
         {
            return 0;
         }
      }
     
      public int GetNumFieldValues(byte fieldNum, string subfieldName) 
      {
         Field field = GetField(fieldNum);

         if (field == null)
         {
            return 0;
         }

         Subfield subfield = field.GetSubfield(subfieldName);
         if ((subfield == null) || (subfield.CanMesgSupport(this)))
         {
            return field.GetNumValues();
         }
         else
         {
            return 0;
         }
      }
           
      public object GetFieldValue(byte fieldNum)
      {
         return GetFieldValue(fieldNum, 0, Fit.SubfieldIndexActiveSubfield);
      }

      public object GetFieldValue(byte fieldNum, int fieldArrayIndex)
      {         
         return GetFieldValue(fieldNum, fieldArrayIndex, Fit.SubfieldIndexActiveSubfield);
      }
     
      public object GetFieldValue(byte fieldNum, int fieldArrayIndex, ushort subFieldIndex) 
      {
         Field field = GetField(fieldNum);

         if (field == null)
         {                        
            return null;
         }

         if (subFieldIndex == Fit.SubfieldIndexActiveSubfield)
         {
            return field.GetValue(fieldArrayIndex, GetActiveSubFieldIndex(fieldNum));
         }
         else 
         {            
            Subfield subfield = field.GetSubfield(subFieldIndex);

            if ((subfield == null) || (subfield.CanMesgSupport(this)))
            {
               return field.GetValue(fieldArrayIndex, subFieldIndex);
            }
            else
            {
               return null;
            }
         }         
      }
      
      public object GetFieldValue(byte fieldNum, int fieldArrayIndex, string subfieldName)
      {
         Field field = GetField(fieldNum);

         if (field == null)
         {
            return null;
         }
   
         Subfield subfield = field.GetSubfield(subfieldName);

         if ((subfield == null) || (subfield.CanMesgSupport(this)))
         {
            return field.GetValue(fieldArrayIndex, subfieldName);
         }
         else
         {
            return null;
         }
      }

      public object GetFieldValue(string name)
      {
         return GetFieldValue(name, 0);
      }

      public object GetFieldValue(string name, int fieldArrayIndex) 
      {        
         Field field = GetField(name, false);

         if (field == null)
         {
            return null;
         }

         Subfield subfield = field.GetSubfield(name);
   
         if ((subfield == null) || (subfield.CanMesgSupport(this)))
         {
            return field.GetValue(fieldArrayIndex, name);
         }
         else
         {
            return null;
         }
      }            

      public void SetFieldValue(byte fieldNum, Object value)
      {
         SetFieldValue(fieldNum, 0, value, Fit.SubfieldIndexActiveSubfield);
      }

      public void SetFieldValue(byte fieldNum, int fieldArrayIndex, Object value)
      {
         SetFieldValue(fieldNum, fieldArrayIndex, value, Fit.SubfieldIndexActiveSubfield);
      }
      
      public void SetFieldValue(byte fieldNum, int fieldArrayIndex, Object value, ushort subfieldIndex)
      {          
         if (subfieldIndex == Fit.SubfieldIndexActiveSubfield)
         {
            subfieldIndex = GetActiveSubFieldIndex(fieldNum);
         }
         else
         {
            Field testField = new Field(this.GetField(fieldNum));
            Subfield subfield = testField.GetSubfield(subfieldIndex);

            if ((subfield != null) && !(subfield.CanMesgSupport(this)))
            {
               return;
            }
         }

         Field field = GetField(fieldNum);

         if (field == null)
         {
            // We normally won't have fields attached to our skeleton message, 
            // as we add values we need to add the fields too based on the mesg,field
            // combo in the profile.              
            field = new Field(Profile.GetMesg(this.Num).GetField(fieldNum));
            if (field.Num == Fit.FieldNumInvalid)
            {
               // If there was no info in the profile our FieldNum will get set to invalid, 
               // at least preserve FieldNum while we know it
               field.Num = fieldNum;
            }
            SetField(field);
         }
         field.SetValue(fieldArrayIndex, value, subfieldIndex);
      }
 
      public void SetFieldValue(byte fieldNum, int fieldArrayIndex, Object value, String subfieldName)
      {
         Field testField =  new Field(this.GetField(fieldNum));
         Subfield subfield = testField.GetSubfield(subfieldName);

         if ((subfield != null) && !(subfield.CanMesgSupport(this)))
         {
            return;
         }
      
         Field field = GetField(fieldNum);

         if (field == null) 
         {
            // We normally won't have fields attached to our skeleton message, 
            // as we add values we need to add the fields too based on the mesg,field
            // combo in the profile.              
            field = new Field(Profile.GetMesg(this.Num).GetField(fieldNum));
            if (field.Num == Fit.FieldNumInvalid)
            {
               // If there was no info in the profile our FieldNum will get set to invalid, 
               // at least preserve FieldNum while we know it
               field.Num = fieldNum;
            }
            SetField(field);
         }
         field.SetValue(fieldArrayIndex, value, subfieldName);
      }

      public void SetFieldValue(String name, Object value)
      {
         SetFieldValue(name, 0, value);
      }

      public void SetFieldValue(String name, int fieldArrayIndex, Object value) 
      {         
         Field testField =  new Field(this.GetField(name));
         Subfield subfield = testField.GetSubfield(name);

         if ((subfield != null) && !(subfield.CanMesgSupport(this)))
         {
            return;
         }

         Field field = GetField(name, false);
   
         if (field == null) 
         {
            field = new Field(Profile.GetMesg(this.Num).GetField(name));
            SetField(field);
         }

         field.SetValue(fieldArrayIndex, value, name);
      }

      public DateTime TimestampToDateTime(uint timestamp)
      {         
         DateTime dateTime = new DateTime(timestamp);
         dateTime.ConvertSystemTimeToUTC(systemTimeOffset);

         return dateTime;
      }

      public DateTime TimestampToDateTime(uint? timestamp)
      {         
         DateTime dateTime = new DateTime(timestamp ?? 0);
         dateTime.ConvertSystemTimeToUTC(systemTimeOffset);

         return dateTime;
      }

      public void ExpandComponents()
      {               
         // We can't modify the fields collection as we are traversing it, save new fields
         // to add after we complete expansion
         List<Field> newFields = new List<Field>();

         foreach (Field myField in fields)
         {
            List<FieldComponent> compsToExpand = null;
            Field containingField = null;

            // Determine the active subfield and expand if it has any components
            ushort activeSubfield = GetActiveSubFieldIndex(myField.Num);

            if (activeSubfield == Fit.SubfieldIndexMainField)
            {
               // There are main field components, expand
               if (myField.components.Count > 0)
               {
                  compsToExpand = myField.components;
                  containingField = myField;
               }
            }
            else
            {
               // There are subfield components, expand
               if (myField.GetSubfield(activeSubfield).Components.Count > 0)
               {
                  compsToExpand = myField.GetSubfield(activeSubfield).Components;
                  containingField = myField;
               }
            }
            if (compsToExpand != null)
            {
               // Comp Decode
               if (containingField.values.Count > 0)
               {
                  int offset = 0;

                  foreach (FieldComponent component in compsToExpand)
                  {
                     if (component.fieldNum != Fit.FieldNumInvalid)
                     {
                        Field destinationField = new Field(Profile.GetMesg(this.Num).GetField(component.fieldNum));

                        // GetBitsValue will not return more bits than the componentField type can hold.  
                        // This means strings are built one letter at a time when using components 
                        // which is a bad idea to start with)
                        long? bitsValue = containingField.GetBitsValue(offset, component.bits, destinationField.Type);
                        if (bitsValue == null)
                        {
                           break;
                        }

                        if (component.accumulate == true)
                        {
                           bitsValue = component.Accumulate(bitsValue.Value);
                        }

                        if (destinationField.IsNumeric())
                        {
                           float fbitsValue = Convert.ToSingle(bitsValue);
                           fbitsValue = ((float)fbitsValue / component.scale) - component.offset;
                           
                           if (this.HasField(destinationField.Num) == true)
                           {
                              this.GetField(destinationField.Num).SetValue(this.GetField(destinationField.Num).values.Count, fbitsValue);
                           }
                           else
                           {
                              destinationField.SetValue(fbitsValue);
                              newFields.Add(destinationField);                              
                           }
                        }
                        // Shouldn't apply scale/offset to string or enum
                        else
                        {
                           object nonNumericBitsValue;
                           // Ensure strings are added as byte[]
                           if ((destinationField.Type & Fit.BaseTypeNumMask) == Fit.String)
                           {
                              nonNumericBitsValue = new byte[] { (byte)bitsValue };
                           }
                           else
                           {
                              nonNumericBitsValue = bitsValue;
                           }
                           if (this.HasField(destinationField.Num) == true)
                           {
                              this.GetField(destinationField.Num).SetValue(this.GetField(destinationField.Num).values.Count, nonNumericBitsValue);
                           }
                           else
                           {
                              destinationField.SetValue(nonNumericBitsValue);
                              newFields.Add(destinationField);                              
                           }
                        }
                     }
                     offset += component.bits;
                  }
               }
            }
         }     
         foreach (Field newField in newFields)
         {
            // Another component added this field during expansion, append the additional values
            if (this.HasField(newField.Num) == true)
            {
               foreach (object newValue in newField.values)
               {
                  this.GetField(newField.Num).SetValue(this.GetField(newField.Num).values.Count, newValue);
               }
            }
            // Add the new field
            else
            {
               this.SetField(newField);
            }
         }           
      }
      #endregion
   }
} // namespace
