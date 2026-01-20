# E-Tax Invoice & E-Receipt  
เกิดขึ้นจากโครงการระบบภาษีและเอกสารธุรกรรมอิเล็กทรอนิกส์ ภายใต้แผนยุทธศาสตร์ National e-Payment ซึ่งเป็นนโยบายของรัฐบาลในการผลักดันให้ระบบการชำระเงิน และระบบภาษีของประเทศก้าวสู่การเป็นเศรษฐกิจดิจิทัล กรมสรรพากรโดยความร่วมมือกับสำนักงานพัฒนาธุรกรรมอิเล็กทรอนิกส์ (สพธอ.) และสำนักงานรัฐบาลอิเล็กทรอนิกส์ (สรอ.) ได้จัดทำข้อมูลอิเล็กทรอนิกส์ เกี่ยวกับใบแจ้งหนี้ ใบรับ และใบกำกับภาษีเพื่อสนับสนุนให้ผู้ที่เกี่ยวข้องในกระบวนการทำธุรกรรมทางอิเล็กทรอนิกส์ ใช้เป็นมาตรฐานในการกำหนดข้อมูลอิเล็กทรอนิกส์ให้สามารถเชื่อมโยงข้อมูลอิเล็กทรอนิกส์ของใบแจ้งหนี้ ใบรับและ ใบกำกับภาษีกับมาตรฐานข้อมูลอิเล็กทรอนิกส์ประเภทอื่น ๆ ได้อย่างมีประสิทธิภาพ สอดคล้องกับมาตรฐานสากล และประมวลรัษฎากร
โดยข้อมูลอิเล็กทรอนิกส์นี้ประกอบด้วยโครงสร้างข้อมูล ดังต่อไปนี้
1. ใบแจ้งหนี้อิเล็กทรอนิกส์ (e-Invoice)
1. ใบรับอิเล็กทรอนิกส์ (e-Receipt)
1. ใบกำกับภาษีอิเล็กทรอนิกส์ (e-Tax Invoice)
1. ใบเพิ่มหนี้/ใบลดหนี้อิเล็กทรอนิกส์ (Debit/Credit Note)
1. ใบกำกับภาษีอย่างย่ออิเล็กทรอนิกส์ (Abbreviated e-Tax Invoice)
1. ใบแจ้งยกเลิกอิเล็กทรอนิกส์ (Cancellation Note)

ซึ่งประกอบด้วยชื่อโครงสร้างข้อมูลและเลข OID ดังต่อไปนี้
1.	ใบแจ้งหนี้อิเล็กทรอนิกส์ (e-Invoice)
*  	Schema: Invoice_CrossIndustryInvoice
* 	OID: 2.16.764.1.4.1.1.2.1.1
2.	ใบรับอิเล็กทรอนิกส์ (e-Receipt)
*  Schema: Receipt_CrossIndustryInvoice
*  OID: 2.16.764.1.4.1.1.3.1.1
3.	ใบกำกับภาษีอิเล็กทรอนิกส์ (e-Tax Invoice)
*  Schema: TaxInvoice_CrossIndustryInvoice
*  OID: 2.16.764.1.4.1.1.4.1.1
4.	ใบเพิ่มหนี้/ใบลดหนี้อิเล็กทรอนิกส์ (Debit/Credit Note)
*  Schema: DebitCreditNote_CrossIndustryInvoice
*  OID: 2.16.764.1.4.1.1.5.1.1
5.	ใบกำกับภาษีอย่างย่ออิเล็กทรอนิกส์ (Abbreviated e-Tax Invoice)
*  Schema: AbbreviatedTaxInvoice_CrossIndustryInvoice
*  OID: 2.16.764.1.4.1.1.6.1.1
6.	ใบแจ้งยกเลิกอิเล็กทรอนิกส์ (Cancellation Note)
*  Schema: CancellationNote_CrossIndustryInvoice
*  OID: 2.16.764.1.4.1.1.7.1.1


# Latest Release

Download version ล่าสุดได้ที่นี่
[E-Tax Invoice & Receipt](https://gitlab.com/etdath-teda-schema/teda-objects/common/e-tax-invoice-receipt/-/tags)

# Contact Us
สามารถติดต่อเราได้ที่ eservice@etda.or.th