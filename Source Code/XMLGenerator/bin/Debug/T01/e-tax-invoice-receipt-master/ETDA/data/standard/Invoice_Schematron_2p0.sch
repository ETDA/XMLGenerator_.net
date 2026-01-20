<?xml version="1.0" encoding="UTF-8"?>
<sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron">
    <sch:ns uri="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:16" prefix="udt"/>
    <sch:ns uri="urn:etda:uncefact:data:standard:Invoice_QualifiedDataType:1" prefix="qdt"/>
    <sch:ns uri="urn:etda:uncefact:data:standard:Invoice_ReusableAggregateBusinessInformationEntity:2" prefix="ram"/>
    <sch:ns uri="urn:etda:uncefact:data:standard:Invoice_CrossIndustryInvoice:2" prefix="rsm"/>

<sch:pattern>
    <sch:rule context="/">
        <sch:report test="not(rsm:Invoice_CrossIndustryInvoice)">No Root Element</sch:report>
    </sch:rule>
   
    <!-- Check scheme version ID -->
    <sch:rule context="rsm:Invoice_CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter">         
        <sch:report test="not((ram:ID/@schemeVersionID = 'v2.0') or (ram:ID/@schemeVersionID = '2.0'))">
            INV-DocumentContext-001 เวอร์ชันของรหัสคู่มือมาตรฐาน (@schemeVersionID) ไม่ถูกต้อง (@schemeVersionID =  'v2.0' or @schemeVersionID =  '2.0' )
            (ExchangedDocumentContext/GuidelineSpecifiedDocumentParameter/ID @schemeVersionID must equal to v2.0 or 2.0)               
        </sch:report>
        
    </sch:rule>

    <!-- Check Document Type Code -->
    <sch:rule context="rsm:Invoice_CrossIndustryInvoice/rsm:ExchangedDocument">
        <sch:report test="not(((ram:TypeCode) = '380'))">
            INV-Document-001 รหัสประเภทเอกสาร (TypeCode) ไม่ถูกต้อง 380 = ใบแจ้งหนี้
            (TypeCode must equal to 380.)
           
        </sch:report>
        <!--<sch:report test="not((ram:Name) = 'ใบแจ้งหนี้')">
            INV-Document-002 ชื่อประเภทเอกสารไม่ถูกต้อง  (ใบแจ้งหนี้)
            (TypeCode must equal to ใบแจ้งหนี้)
        </sch:report>-->
        
        
        <!--<sch:report test="(((ram:TypeCode) = '380')and((ram:Name) != 'ใบแจ้งหนี้'))">
            INV-Document-003 รหัสประเภทเอกสาร (ExchangedDocument/TypeCode) และชื่อประเภทเอกสาร (ExchangedDocument/Name) ไม่สอดคล้องกัน
            (TypeCode and TypeName Not Match.)
        </sch:report>-->
       
    </sch:rule>  
    
    
    <!-- SellerTradeParty Rule -->
    <sch:rule context="rsm:Invoice_CrossIndustryInvoice/rsm:SupplyChainTradeTransaction/ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty">
        <sch:report test="not((ram:Name)!='')">
            INV-SellerTradeParty-001 ต้องระบุชื่อผู้ขาย
            (Name must be present since SellerTradeParty is present.)
        </sch:report>
        
      </sch:rule>
 
</sch:pattern>

</sch:schema>