# Dictionary renderer toolkit
Extend ArcGIS Pro, the ArcGIS API for JavaScript, and the ArcGIS Runtime SDKs by creating custom dictionary renderers. Dictionary renderers allow complex symbology specifications, such as those using in military symbology, to be configured and drawn in maps. This toolkit provides resources to support modifying and creating symbol dictionaries for use with the dictionary renderer to suit your symbolization requirements.

* [Understanding the dictionary](/docs/understanding-the-dictionary.md)
* [Tips for creating custom dictionaries](/docs/tips-for-creating-custom-dictionaries.md)
* [Working with different field types](/docs/working-with-different-field-types.md)
* [Troubleshooting drawing issues](/docs/troubleshooting_drawing_issues.md)
* [Add a new identity to MIL-STD-2525C](/docs/add-a-new-identity-to-MIL-STD-2525C.md)
* [Add a new symbol set to MIL-STD-2525D](/docs/add-a-new-symbol-set-to-MIL-STD-2525D.md)
* [Add a configuration for country indicator for MIL-STD-2525C](/docs/add-a-configuration-for-country-indicator-for-MIL-STD-2525C.md)
* [Assign color by team for MIL-STD-2525D](/docs/assign_color_by_team_for_MIL-STD-2525D.md)
* [Working with overrides in dictionaries](/docs/working-with-overrides.md)
* [Editing labels in military styles](/docs/editing-label-symbols-in-military-styles.md)
* [Upgrading dictionary styles from a previous version](/docs/upgrading-dictionary-styles-from-a-previous-version.md)

## Requirements

Dictionaries generated using this toolkit can be used in ArcGIS Pro 2.5 or higher and in ArcGIS Runtime SDKs 100.7 or higher.

The dictionaries can be shared as web styles for use with ArcGIS Runtime SDKs 100.10 or higher, ArcGIS Pro 2.6 or higher, and ArcGIS API for JavaScript 4.13 or higher.

_Note: The ArcGIS API for JavaScript version 4.16 and higher support point, line, and polygon symbols. The ArcGIS API for JavaScript versions 4.13 through 4.15 support point symbols only._

Previous versions of this toolkit allowed for the creation of older version dictionaries. These are archived as branches of this repository. The table below shows the branches available and the versions of the dictionary, ArcGIS Pro and ArcGIS Runtime SDK that they correspond to. It is recommended that dictionaries be upgraded to version 4.0.0, to achieve the full functionality of the dictionary. See [Upgrading dictionary styles from a previous version](docs/upgrading-dictionary-styles-from-a-previous-version.md) for steps to upgrade.



| Branch | Notes | ArcGIS Pro | ArcGIS Runtime SDK | ArcGIS Maps SDK for JavaScript | Dictionary Version |
| -------| ----- | ---------- | ------------------ | ------------------------------ | ------------------ |
|master |	Dictionary version 4.0.0 | Pro 3.2 or higher	| 200.2 or higher	| 4.27 or higher | 4.0.0 |
|[release/3.0.0](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/3.0.0) | Add-ins are for ArcGIS Pro 3x, dictionary version 3.0.0	| Pro 3.0 or higher	| 100.7 or higher | 4.13 or higher |	3.0.0 |
|[release/2.9.0](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/2.9.0) |	Add-ins are for ArcGIS Pro 2x |	Pro 2.5 - Pro 2.9 |	100.7 or higher	| 4.13 or higher | 3.0.0 |
|[release/2.0.0](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/2.0.0) |	Dictionary version 2.0.0 |Pro 2.4 |	100.6	| NA | 2.0.0 |






See the [ArcGIS Pro system requirements](https://pro.arcgis.com/en/pro-app/get-started/arcgis-pro-system-requirements.htm) for requirements to run ArcGIS Pro.

In order to build Add-Ins provided in this repository, see the [ArcGIS Pro 2.5 SDK for .NET requirements](https://github.com/esri/arcgis-pro-sdk/wiki#requirements).

## Contributing

Esri welcomes contributions from anyone and everyone. For more information, see our [guidelines for contributing](https://github.com/esri/contributing).

## Issues
Find a bug or want to request a new feature? Let us know by submitting an issue.

## Licensing
Copyright 2023 Esri

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at:

http://www.apache.org/licenses/LICENSE-2.0.

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

A copy of the license is available in the repository's [license.txt](https://github.com/Esri/arcgis-pro-metadata-toolkit/blob/master/license.txt) file.
