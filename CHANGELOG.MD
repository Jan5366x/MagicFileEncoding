## v2.0.0 - ?? ??? ????
* Switch to Net 6.0
* Enabled nullable reference types
* Add SYSLIB0001 UTF-7 Encoding safety check
* Remove IO Tools from public surface since they are off-topic internal logic which is object to (breaking) changes

## v1.1.0 - 13 Feb 2021
* **Breaking Change!** Adjusted public api surface to static access
* **Breaking Change!** Additional encodings are now null if not found in the code pages
* Added more tests
* Added more additional encodings
* Improved documentation

**disclaimer:** This version contains breaking changes without major version bump,
this will be avoided in future releases

## 1.0.0 - 28 Sep 2020
* Added basic feature set