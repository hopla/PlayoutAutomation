﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace TAS.Client.Common
{
    public abstract class EditViewModelBase<M> : ViewModels.ViewmodelBase 
    {
        public readonly M Model;
        public readonly UserControl _editor;
        public EditViewModelBase(M model, UserControl editor)
        {
            Model = model;
            _editor = editor;
            Load(model);
            _modified = false;
            editor.DataContext = this;
        }

        protected bool _modified;
        protected virtual void OnModified(){}

        [XmlIgnore]
        public virtual bool Modified
        {
            get { return _modified; }
            protected set
            {
                if (base.SetField(ref _modified, value, "Modified"))
                    OnModified();
            }
        }

        protected override bool SetField<T>(ref T field, T value, string propertyName)
        {
            bool modified = base.SetField<T>(ref field, value, propertyName);
            if (modified) Modified = true;
            return modified;
        }

        protected virtual void Load(object source)
        {
            IEnumerable<PropertyInfo> copiedProperties = this.GetType().GetProperties().Where(p => p.CanWrite);
            foreach (PropertyInfo copyPi in copiedProperties)
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(copyPi.Name);
                if (sourcePi != null)
                    copyPi.SetValue(this, sourcePi.GetValue(source, null), null);
            }
        }

        public virtual void Save(object destObject = null)
        {
            if (Modified && Model != null
                || destObject != null)
            {
                PropertyInfo[] copiedProperties = this.GetType().GetProperties();
                foreach (PropertyInfo copyPi in copiedProperties)
                {
                    PropertyInfo destPi = (destObject ?? Model).GetType().GetProperty(copyPi.Name);
                    if (destPi != null)
                    {
                        if (destPi.GetValue(destObject ?? Model, null) != copyPi.GetValue(this, null)
                            && destPi.CanWrite)
                            destPi.SetValue(destObject ?? Model, copyPi.GetValue(this, null), null);
                    }
                }
                Modified = false;
            }
        }

        [XmlIgnore]
        public UserControl Editor { get { return _editor; } }

    }
}
