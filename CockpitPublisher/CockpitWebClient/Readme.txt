To debug with a valid MDGuids Querystring Parameter do the following:
 - Go to project CockpitPublisher.Common.Tests
 - In Test Method SerializeListAsBase64String_GuidWith1ElementIsSerialized_SerializeAndDeserializeWorkTogether() you can serialize an MDGuid in a proper way
 - Go to project CockpitWebClient
 - Choose Eigenschaften/Startaktion/Bestimmte Seite:
 - Insert MyCockpit.aspx?MDGuids=
   followed by the serialized string... 
   e.g. MyCockpit.aspx?MDGuids=AAEAAAD/////AQAAAAAAAAAEAQAAAH9TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24GAAAICAkCAAAAAQAAAAEAAAARAgAAAAQAAAAGAwAAACRDOTQyRUY5Qi1BNDU1LTQ5QkUtQjdGQi01NTA3RkNEMkYxQzANAws=