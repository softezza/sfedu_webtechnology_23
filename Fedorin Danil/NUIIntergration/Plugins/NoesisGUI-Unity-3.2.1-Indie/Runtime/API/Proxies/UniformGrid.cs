//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class UniformGrid : Panel {
  internal new static UniformGrid CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new UniformGrid(cPtr, cMemoryOwn);
  }

  internal UniformGrid(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(UniformGrid obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public UniformGrid() {
  }

  protected override IntPtr CreateCPtr(Type type, out bool registerExtend) {
    registerExtend = false;
    return NoesisGUI_PINVOKE.new_UniformGrid();
  }

  public static DependencyProperty FirstColumnProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.UniformGrid_FirstColumnProperty_get();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty ColumnsProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.UniformGrid_ColumnsProperty_get();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty RowsProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.UniformGrid_RowsProperty_get();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public int FirstColumn {
    set {
      NoesisGUI_PINVOKE.UniformGrid_FirstColumn_set(swigCPtr, value);
    }
    get {
      int ret = NoesisGUI_PINVOKE.UniformGrid_FirstColumn_get(swigCPtr);
      return ret;
    }
  }

  public int Columns {
    set {
      NoesisGUI_PINVOKE.UniformGrid_Columns_set(swigCPtr, value);
    }
    get {
      int ret = NoesisGUI_PINVOKE.UniformGrid_Columns_get(swigCPtr);
      return ret;
    }
  }

  public int Rows {
    set {
      NoesisGUI_PINVOKE.UniformGrid_Rows_set(swigCPtr, value);
    }
    get {
      int ret = NoesisGUI_PINVOKE.UniformGrid_Rows_get(swigCPtr);
      return ret;
    }
  }

}

}

