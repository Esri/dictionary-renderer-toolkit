# Upgrading dictionary styles from a previous version

## Versions
We recommend dictionaries versions of `3.0.0` or `4.0.0` for full compatibility with the web styles and the ArcGIS Maps SDK for JavaScript.  Both dictionary versions are fully compatible with those specification.  Version `4.0.0` has additional functionality for supporting native data types without converting the values to strings.  See [Working with different field types](docs/working-with-different-field-types.md) for additional details.

Version `2.0.0` dictionaries cannot be shared as web styles and are not compatible with the ArcGIS Maps SDK for JavaScript.  It is highly recommended to upgrade these dictionaries.


| Dictionary Version  | Version Notes | ArcGIS Pro | ArcGIS Runtime SDK | ArcGIS Maps SDK for JavaScript | Branch |
| ------------------ | ----- | ---------- | ------------------ | ------------- |------------------ |
|4.0.0 |	Support for native data types | Pro 3.2 or higher	| 200.2 or higher	| 4.27 or higher | master |
|3.0.0 | Usage of $feature and $config to be more Arcade compliant	| Pro 3.0 or higher	| 100.7 or higher | 4.13 or higher |	[release/3.0.0](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/3.0.0)|
|2.0.0 |	Initial version |Pro 2.4 |	100.6	| NA | [release/2.0.0](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/2.0.0)|

## Upgrading 3.0.0 to 4.0.0
Upgrading the dictionary to version `4.0.0` is optional but does offer more control over the functionality of the operators used in the script because the native data types are preserved.

1. The first change is that the value of key `dictionary_version` in the `meta` table must be changed from `3.0.0` to `4.0.0`.

2. Depending on the operators used in the script it may be necessary to make adjustments in the script.  Comparison operators, like == and !=, always consider different data types as not equal.  If these are used in the script they will need to be adjusted.  

An example of this can be seen in the [Park Amenities](../dictionary_examples/Park_Amenities) dictionary.
In the script, icons are shown for the different amenities by checking if the attribute value does not equal zero. The != operator is used for this comparison.  To upgrade to `4.0.0` it is necessary to remove the single quotes or the comparison will fail and symbols will unexpectedly draw.

In `3.0.0` the script would be
`if ($feature.TENNIS != '0')`    which is an explicit string comparison.
  
In `4.0.0` the script would be
  `if ($feature.TENNIS != 0)`    which is a number comparison. 

If you are working with a custom dictionary that is based on the Joint Military Symbology or NATO Joint Military Symbology specifications the best way to identify changes that are needed in the script is to compare your custom script to the `4.0.0` versions of the dictionaries available from ArcGIS Online. The `4.0.0` script is more robust for different database schemes; however, the logic of the military specifications requires the symbol ID codes to be considered as string.  Modifications were made to ensure the database is properly converted to what the script requires.


## Upgrading 2.0.0 to 3.0.0

Upgrading the dictionary requires two steps and are typically done with manual edits of the SQLite database:

1. The first change that must be made is that the Arcade script value stored in the style `meta` table key `dictionary_script` must be updated to reflect input parameters being passed in using `$feature` and `$config`. Previously, field values were passed in as `$fieldname` and a configuration values were passed in as `$configurationentryname`. In `3.0.0` dictionaries, all feature values are passed in via a `$feature` feature object and configuration values are passed in with a `$config` dictionary. So, the above example would be passed in now as `$feature.fieldname` and `$config.configurationentryname`. For the mil2525d dictionary, an example of this change would be `$sidc` changing to `$feature.sidc` and `$frame` changing to `$config.frame`.

1. The final change is that the value of key `dictionary_version` in the `meta` table must be changed from `2.0.0` to `3.0.0`.

Once the above steps are complete and your SQLite database is saved, your dictionary is upgraded.
