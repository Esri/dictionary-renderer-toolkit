# Dictionary renderer toolkit
Extend ArcGIS Pro, the ArcGIS API for JavaScript, and the ArcGIS Runtime SDKs by creating custom dictionary renderers. Dictionary renderers allow complex symbology specifications, such as those using in military symbology, to be configured and drawn in maps. This toolkit provides resources to support modifying and creating symbol dictionaries for use with the dictionary renderer to suit your symbolization requirements.

* [Understanding the dictionary](/docs/understanding-the-dictionary.md)
* [Tips for creating custom dictionaries](/docs/tips-for-creating-custom-dictionaries.md)
* [Add a new identity to MIL-STD-2525C](/docs/add-a-new-identity-to-MIL-STD-2525C.md)
* [Add a new symbol set to MIL-STD-2525D](/docs/add-a-new-symbol-set-to-MIL-STD-2525D.md)
* [Add a configuration for country indicator for MIL-STD-2525C](/docs/add-a-configuration-for-country-indicator-for-MIL-STD-2525C.md)
* [Working with overrides in dictionaries](working-with-overrides.md)
* [Upgrading dictionary styles from a previous version](pgrading-dictionary-styles-from-a-previous-version.md)

## Requirements

Dictionaries generated using this toolkit can be used in ArcGIS Pro 2.5 or higher and ArcGIS Runtime SDKs 100.7 or higher and published as web styles for use with the ArcGIS API for JavaScript 4.13 or higher. Note: The ArcGIS API for JavaScript only supports dictionary symbology for point feature layers at this time.

A previous version of this toolkit allowed for creation of dictionary styles that worked with ArcGIS Pro 2.4 and ArcGIS Runtime SDKs 100.6. It is archived as a [branch](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/2.0.0) of this repository and it's recommended that dictionaries be upgraded to version 3.0.0, which this toolkit documents from that previous 2.0.0 version. See [Upgrading dictionary styles from a previous version](docs/upgrading-dictionary-styles-from-a-previous-version.md) for steps to upgrade.

See the [ArcGIS Pro system requirements](https://pro.arcgis.com/en/pro-app/get-started/arcgis-pro-system-requirements.htm) for requirements to run ArcGIS Pro.

In order to build Add-Ins provided in this repository, see the [ArcGIS Pro 2.5 SDK for .NET requirements](https://github.com/esri/arcgis-pro-sdk/wiki#requirements).

## Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

## Issues
Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2020 Esri

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at:

http://www.apache.org/licenses/LICENSE-2.0.

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

A copy of the license is available in the repository's [license.txt](https://github.com/Esri/arcgis-pro-metadata-toolkit/blob/master/license.txt) file.