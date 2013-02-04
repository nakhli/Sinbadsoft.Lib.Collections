// <copyright file="AssemblyInfo.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/03/01</date>

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Sinbadsoft.Lib.Collections")]
[assembly: AssemblyDescription("Priority queue, reversible dictionary, reversible sorted dictionary, dynamic multidimensional array, heap, heapsort etc.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sinbadsoft")]
[assembly: AssemblyProduct("Sinbadsoft.Lib.Collections")]
[assembly: AssemblyCopyright("Copyright © Sinbadsoft 2009-2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COMset the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9d045fdd-9b90-41ce-896b-ef8f1e3ea89b")]

// Version information for an assembly consists of the following four values:
// Major Version
// Minor Version 
// Build Number
// Revision
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("0.3.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Mark assembly CLS Compliant
[assembly: CLSCompliant(true)]

// Unit tests as friend assembly
[assembly:
        InternalsVisibleTo(
                "Sinbadsoft.Lib.Collections.Tests, PublicKey="
                + "0024000004800000940000000602000000240000525341310004000001000100732bfd900f144f"
                + "6af693e99f1493ca778e7715d46ba3b9ba537237e3e4ff1ab447b0ded0a9afa59cabdbe7c0402b"
                + "14f1ff05cb8350a71f0f137c98d8236d2a36ba311fe2346e5bec1337986b6b05caa946e3345360"
                + "cd9c188de7fb5e3ee1e7969256aa3031ece905b5c41a7d54b094e3a5ef35976df9b89c8676cc0f"
                + "1e957fbc")]