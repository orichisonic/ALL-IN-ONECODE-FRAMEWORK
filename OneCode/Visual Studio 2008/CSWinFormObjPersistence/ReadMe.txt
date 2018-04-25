================================================================================
       WINDOWS FORMS APPLICATION : CSWinFormObjPersistence Project Overview
       
                        Object Persistance Sample
                        
===============================================================================

/////////////////////////////////////////////////////////////////////////////
Use:

The Object Persistance sample demonstrates how to persist an object's data 
between instances, so that the data can be restored the next time the object 
is instantiated. 
   

/////////////////////////////////////////////////////////////////////////////
Code Logic:

This sample uses a binaray file "Customer.bin" to store the information.

1. Create a class names Customer with some properties, and make the whole class
   as serializable by adding the Serializable attribute on top of the class 
   definition. 
   If you do not want to persist a field, mark it as NonSerialized. 

2. In the Form.Load event,check if the Customer.bin file exists, it the file 
   exists, use BinaryFormatter.Deserialize method to retrieve the data stored 
   in it, otherwise create a new instance of Customer class.
   
3. In the Form.FormClosing event,use BinaryFormatter.Serialize method to 
   serialize the customer information to the Customer.bin file.


/////////////////////////////////////////////////////////////////////////////
References:
   
1. Walkthrough: Persisting an Object in Visual Basic .NET
   http://msdn.microsoft.com/en-us/library/aa984468(VS.71).aspx
   
2. Windows Forms General FAQ.
   http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/77a66f05-804e-4d58-8214-0c32d8f43191
   

/////////////////////////////////////////////////////////////////////////////
