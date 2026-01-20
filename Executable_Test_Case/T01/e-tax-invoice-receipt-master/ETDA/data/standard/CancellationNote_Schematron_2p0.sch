<?xml version="1.0" encoding="UTF-8"?>
<sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron">
    <sch:ns uri="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:16" prefix="udt"/>
    <sch:ns uri="urn:etda:uncefact:data:standard:QualifiedDataType:1" prefix="qdt"/>
    <sch:ns uri="urn:etda:uncefact:data:standard:CancellationNote_ReusableAggregateBusinessInformationEntity:2" prefix="ram"/>
    <sch:ns uri="urn:etda:uncefact:data:standard:CancellationNote_CrossIndustryInvoice:2" prefix="rsm"/>

<sch:pattern>
    <sch:rule context="/">
        <sch:report test="not(rsm:CancellationNote_CrossIndustryInvoice)">No Root Element</sch:report>
    </sch:rule>
    
    <!-- Check scheme version ID -->
    <sch:rule context="rsm:CancellationNote_CrossIndustryInvoice/rsm:ExchangedDocumentContext/ram:GuidelineSpecifiedDocumentContextParameter">         
        <sch:report test="not((ram:ID/@schemeVersionID = 'v2.0') or (ram:ID/@schemeVersionID = '2.0'))">
            CLN-DocumentContext-001 เวอร์ชันของรหัสคู่มือมาตรฐาน (@schemeVersionID) ไม่ถูกต้อง (@schemeVersionID =  'v2.0' or @schemeVersionID =  '2.0' )
            (ExchangedDocumentContext/GuidelineSpecifiedDocumentParameter/ID @schemeVersionID must equal to v2.0 or 2.0)               
        </sch:report>
        
    </sch:rule>
    <!-- Check Document Type Code -->
    <sch:rule context="rsm:CancellationNote_CrossIndustryInvoice/rsm:ExchangedDocument">
        <sch:report test="not(((ram:TypeCode) = 'T07'))">
            CLN-Document-001 รหัสประเภทเอกสารไม่ถูกต้อง T07 = ใบแจ้งยกเลิก
            (TypeCode must equal to T07.)    
        </sch:report>

     <!--

        <sch:report test="(((ram:TypeCode) = 'T07')and((ram:Name) != 'ใบแจ้งยกเลิก'))">
            CLN-Document-002 รหัสประเภทเอกสารและชื่อประเภทเอกสารไม่สอดคล้องกัน
            (TypeCode and Name Not Match.)
        </sch:report>
        
        <sch:report test="(( (ram:PurposeCode) or (ram:Purpose) or (ram:IncludedCINote/ram:Content) ) and (not( (ram:PurposeCode) and (ram:Purpose) and (ram:IncludedCINote/ram:Content) ) ))">
            ไม่พบข้อมูรหัสสาเหตุ สาเหตุ และหมายเหตุการออกเอกสาร (tiv-nodata-001 : ram:PurposeCode, ram:Purpose and ram:IncludedCINote/ram:Content must be present because AdditionalReferencedDocument is present.)
        </sch:report>  
     -->
    </sch:rule>
</sch:pattern>

</sch:schema>