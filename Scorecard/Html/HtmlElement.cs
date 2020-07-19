/*
 * Copyright (C) 2006 Christian Birkl <Christian.Birkl at gmail.com> 
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 **/
using System;
using System.Xml;
using System.Reflection;

using Cb.Web;
using Cb.Web.Css;
using Cb.Web.Html;
using Cb.Web.Html.Specialized;
using Cb.Web.Scripting;

namespace Cb.Web.Html {
	
	/// <summary>
	/// Represents a single html element
	/// </summary>
	public class HtmlElement : XmlElement, HtmlNode {

		private CssStyleCollection m_Styles = new CssStyleCollection();

		/// <summary>
		/// Styles
		/// </summary>
		public CssStyleCollection Styles {
			get { return m_Styles; }
		}

		/// <summary>
		/// Owner document
		/// </summary>
		public new HtmlDocument OwnerDocument {
			get { return (HtmlDocument)base.OwnerDocument; }			
		}

		/// <summary>
		/// Parent node
		/// </summary>
		public new HtmlNode ParentNode {
			get { return (HtmlNode)base.ParentNode; }
		}

		/// <summary>
		/// Parent element
		/// </summary>
		public HtmlElement ParentElement {
			get { return ParentNode as HtmlElement; }
		}

		/// <summary>
		/// Returns the value of the name attribute
		/// </summary>
		public override string Name {
			get { return GetAttribute("name"); }
		}

		/// <summary>
		/// Creates a new html element
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <param name="doc"></param>
		protected internal HtmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc) 
			: base(prefix, localName, namespaceURI, doc)
		{
			m_ScriptObjectImpl = new ScriptObjectImpl(this);
		}

		/// <summary>
		/// Returns a member of the given dom level
		/// </summary>
		/// <param name="level">supported dom level</param>
		/// <param name="name">member</param>
		/// <returns>member</returns>
		public virtual MemberInfo[] GetDomMember(ScriptDomLevel level, string name) {
			switch (name) {
				case "getElementById":	return GetType().GetMember("GetElementById");		
				case "styles":			return GetType().GetMember("Styles");
			}
			return null;
		}

		/// <summary>
		/// Returns all element, which have a attribute 'name' with the given value
		/// </summary>
		/// <param name="name">name</param>
		/// <returns>list of elements</returns>
		public HtmlElementList GetElementsByName(string name) {
			return new HtmlElementList(SelectNodes(string.Format(".//*[@name = '{0}']", name)));
		}

		/// <summary>
		/// Returns all element, which match the given name
		/// </summary>
		/// <param name="name">name</param>
		/// <returns>list of elements</returns>
		public new HtmlElementList GetElementsByTagName(string name) {
			return HtmlDomHelper.GetElementsByTagName(this, name);
		}

		/// <summary>
		/// Returns the index of the given child
		/// </summary>
		/// <param name="child">child</param>
		/// <returns>index</returns>
		public int IndexOf(HtmlNode child) {
			return HtmlDomHelper.IndexOf(this, child);
		}

		/// <summary>
		/// Clicks this element
		/// </summary>
		public virtual void Click() {
			;
		}

		#region ScriptObject Impl

		/// <summary>
		/// Our internal implemention of the scripting interface
		/// </summary>
		internal ScriptObjectImpl m_ScriptObjectImpl = null;

		/// <summary>
		/// Sets the expando value 'key' to 'value'
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="value">value</param>
		public void SetExpando(object key, object value) {
			m_ScriptObjectImpl.SetExpando(key, value);
		}

		/// <summary>
		/// Removes the expando value for the specified key
		/// </summary>
		/// <param name="key">key</param>
		public void DeleteExpando(object key) {
			m_ScriptObjectImpl.DeleteExpando(key);
		}

		/// <summary>
		/// Returns the expando value for the given key
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>value</returns>
		public object GetExpando(object key) {
			return m_ScriptObjectImpl.GetExpando(key);
		}

		/// <summary>
		/// Invokes the given member
		/// </summary>
		/// <param name="name"></param>
		/// <param name="invokeAttr"></param>
		/// <param name="binder"></param>
		/// <param name="target"></param>
		/// <param name="args"></param>
		/// <param name="modifiers"></param>
		/// <param name="culture"></param>
		/// <param name="namedParameters"></param>
		/// <returns></returns>
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters) {
			return m_ScriptObjectImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}
        
		/// <summary>
		/// Returns the given member
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) {            
			return m_ScriptObjectImpl.GetMember(name, bindingAttr);
		}

		/// <summary>
		/// Returns the underlying system type
		/// </summary>
		public Type UnderlyingSystemType {
			get { return m_ScriptObjectImpl.UnderlyingSystemType; }
		}

		/// <summary>
		/// Returns the property, which matches the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <param name="binder"></param>
		/// <param name="returnType"></param>
		/// <param name="types"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers) {
			return m_ScriptObjectImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		/// <summary>
		/// Returns the property, which matches the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>		
		/// <returns></returns>
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetProperty(name, bindingAttr);
		}

		/// <summary>
		/// Returns all fields, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public FieldInfo[] GetFields(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetFields(bindingAttr);
		}

		/// <summary>
		/// Returns the field, which matches the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public FieldInfo GetField(string name, BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetField(name, bindingAttr);
		}

		/// <summary>
		/// Returns all members, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public MemberInfo[] GetMembers(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetMembers(bindingAttr);
		}

		/// <summary>
		/// Returns all properties, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public PropertyInfo[] GetProperties(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetProperties(bindingAttr);
		}

		/// <summary>
		/// Returns the method, which match the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetMethod(name, bindingAttr);
		}

		/// <summary>
		/// Returns the method, which match the specified arguments
		/// </summary>
		/// <param name="name">name</param>
		/// <param name="bindingAttr">bindingAttr</param>
		/// <param name="binder"></param>		
		/// <param name="types"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) {
			return m_ScriptObjectImpl.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		/// <summary>
		/// Returns all method, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr">bindingAttr</param>		
		/// <returns>list of method infos</returns>
		public MethodInfo[] GetMethods(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetMethods(bindingAttr);
		}

		#endregion

	}

}
