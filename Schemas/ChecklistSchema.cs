
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class CHECKLIST
{

    private CHECKLISTASSET aSSETField;

    private CHECKLISTISTIG[] sTIGSField;

    /// <remarks/>
    public CHECKLISTASSET ASSET
    {
        get
        {
            return this.aSSETField;
        }
        set
        {
            this.aSSETField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("iSTIG", IsNullable = false)]
    public CHECKLISTISTIG[] STIGS
    {
        get
        {
            return this.sTIGSField;
        }
        set
        {
            this.sTIGSField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CHECKLISTASSET
{

    private string rOLEField;

    private string aSSET_TYPEField;

    private object hOST_NAMEField;

    private object hOST_IPField;

    private object hOST_MACField;

    private object hOST_FQDNField;

    private object tECH_AREAField;

    private ushort tARGET_KEYField;

    private bool wEB_OR_DATABASEField;

    private object wEB_DB_SITEField;

    private object wEB_DB_INSTANCEField;

    /// <remarks/>
    public string ROLE
    {
        get
        {
            return this.rOLEField;
        }
        set
        {
            this.rOLEField = value;
        }
    }

    /// <remarks/>
    public string ASSET_TYPE
    {
        get
        {
            return this.aSSET_TYPEField;
        }
        set
        {
            this.aSSET_TYPEField = value;
        }
    }

    /// <remarks/>
    public object HOST_NAME
    {
        get
        {
            return this.hOST_NAMEField;
        }
        set
        {
            this.hOST_NAMEField = value;
        }
    }

    /// <remarks/>
    public object HOST_IP
    {
        get
        {
            return this.hOST_IPField;
        }
        set
        {
            this.hOST_IPField = value;
        }
    }

    /// <remarks/>
    public object HOST_MAC
    {
        get
        {
            return this.hOST_MACField;
        }
        set
        {
            this.hOST_MACField = value;
        }
    }

    /// <remarks/>
    public object HOST_FQDN
    {
        get
        {
            return this.hOST_FQDNField;
        }
        set
        {
            this.hOST_FQDNField = value;
        }
    }

    /// <remarks/>
    public object TECH_AREA
    {
        get
        {
            return this.tECH_AREAField;
        }
        set
        {
            this.tECH_AREAField = value;
        }
    }

    /// <remarks/>
    public ushort TARGET_KEY
    {
        get
        {
            return this.tARGET_KEYField;
        }
        set
        {
            this.tARGET_KEYField = value;
        }
    }

    /// <remarks/>
    public bool WEB_OR_DATABASE
    {
        get
        {
            return this.wEB_OR_DATABASEField;
        }
        set
        {
            this.wEB_OR_DATABASEField = value;
        }
    }

    /// <remarks/>
    public object WEB_DB_SITE
    {
        get
        {
            return this.wEB_DB_SITEField;
        }
        set
        {
            this.wEB_DB_SITEField = value;
        }
    }

    /// <remarks/>
    public object WEB_DB_INSTANCE
    {
        get
        {
            return this.wEB_DB_INSTANCEField;
        }
        set
        {
            this.wEB_DB_INSTANCEField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CHECKLISTISTIG
{

    private CHECKLISTISTIGSI_DATA[] sTIG_INFOField;

    private CHECKLISTISTIGVULN[] vULNField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("SI_DATA", IsNullable = false)]
    public CHECKLISTISTIGSI_DATA[] STIG_INFO
    {
        get
        {
            return this.sTIG_INFOField;
        }
        set
        {
            this.sTIG_INFOField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("VULN")]
    public CHECKLISTISTIGVULN[] VULN
    {
        get
        {
            return this.vULNField;
        }
        set
        {
            this.vULNField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CHECKLISTISTIGSI_DATA
{

    private string sID_NAMEField;

    private string sID_DATAField;

    /// <remarks/>
    public string SID_NAME
    {
        get
        {
            return this.sID_NAMEField;
        }
        set
        {
            this.sID_NAMEField = value;
        }
    }

    /// <remarks/>
    public string SID_DATA
    {
        get
        {
            return this.sID_DATAField;
        }
        set
        {
            this.sID_DATAField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CHECKLISTISTIGVULN
{

    private CHECKLISTISTIGVULNSTIG_DATA[] sTIG_DATAField;

    private string sTATUSField;

    private object fINDING_DETAILSField;

    private object cOMMENTSField;

    private object sEVERITY_OVERRIDEField;

    private object sEVERITY_JUSTIFICATIONField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("STIG_DATA")]
    public CHECKLISTISTIGVULNSTIG_DATA[] STIG_DATA
    {
        get
        {
            return this.sTIG_DATAField;
        }
        set
        {
            this.sTIG_DATAField = value;
        }
    }

    /// <remarks/>
    public string STATUS
    {
        get
        {
            return this.sTATUSField;
        }
        set
        {
            this.sTATUSField = value;
        }
    }

    /// <remarks/>
    public object FINDING_DETAILS
    {
        get
        {
            return this.fINDING_DETAILSField;
        }
        set
        {
            this.fINDING_DETAILSField = value;
        }
    }

    /// <remarks/>
    public object COMMENTS
    {
        get
        {
            return this.cOMMENTSField;
        }
        set
        {
            this.cOMMENTSField = value;
        }
    }

    /// <remarks/>
    public object SEVERITY_OVERRIDE
    {
        get
        {
            return this.sEVERITY_OVERRIDEField;
        }
        set
        {
            this.sEVERITY_OVERRIDEField = value;
        }
    }

    /// <remarks/>
    public object SEVERITY_JUSTIFICATION
    {
        get
        {
            return this.sEVERITY_JUSTIFICATIONField;
        }
        set
        {
            this.sEVERITY_JUSTIFICATIONField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CHECKLISTISTIGVULNSTIG_DATA
{

    private string vULN_ATTRIBUTEField;

    private string aTTRIBUTE_DATAField;

    /// <remarks/>
    public string VULN_ATTRIBUTE
    {
        get
        {
            return this.vULN_ATTRIBUTEField;
        }
        set
        {
            this.vULN_ATTRIBUTEField = value;
        }
    }

    /// <remarks/>
    public string ATTRIBUTE_DATA
    {
        get
        {
            return this.aTTRIBUTE_DATAField;
        }
        set
        {
            this.aTTRIBUTE_DATAField = value;
        }
    }
}

