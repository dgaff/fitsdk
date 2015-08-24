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


#if !defined(FIT_COURSE_MESG_HPP)
#define FIT_COURSE_MESG_HPP

#include "fit_mesg.hpp"

namespace fit
{

class CourseMesg : public Mesg
{
   public:
      CourseMesg(void) : Mesg(Profile::MESG_COURSE)
      {
      }

      CourseMesg(const Mesg &mesg) : Mesg(mesg)
      {
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns sport field
      ///////////////////////////////////////////////////////////////////////
      FIT_SPORT GetSport(void) const
      {
         return GetFieldENUMValue(4, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set sport field
      ///////////////////////////////////////////////////////////////////////
      void SetSport(FIT_SPORT sport)
      {
         SetFieldENUMValue(4, sport, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns name field
      ///////////////////////////////////////////////////////////////////////
      FIT_WSTRING GetName(void) const
      {
         return GetFieldSTRINGValue(5, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set name field
      ///////////////////////////////////////////////////////////////////////
      void SetName(FIT_WSTRING name)
      {
         SetFieldSTRINGValue(5, name, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Returns capabilities field
      ///////////////////////////////////////////////////////////////////////
      FIT_COURSE_CAPABILITIES GetCapabilities(void) const
      {
         return GetFieldUINT32ZValue(6, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

      ///////////////////////////////////////////////////////////////////////
      // Set capabilities field
      ///////////////////////////////////////////////////////////////////////
      void SetCapabilities(FIT_COURSE_CAPABILITIES capabilities)
      {
         SetFieldUINT32ZValue(6, capabilities, 0, FIT_SUBFIELD_INDEX_MAIN_FIELD);
      }

};

} // namespace fit

#endif // !defined(FIT_COURSE_MESG_HPP)
