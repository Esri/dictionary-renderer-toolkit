# Upgrading dictionary styles from a previous version

## Versions
This toolkit defines how to create a dictionary of version `3.0.0`. [A previous version of the toolkit](https://github.com/Esri/dictionary-renderer-toolkit/tree/release/2.0.0), documented version `2.0.0` and we recommend upgrading dictionary styles to the `3.0.0` specification for full compatibility with the web styles and the ArcGIS API for JavaScript.

## Upgrading the style

Upgrading the dictionary requires two steps and are typically done with manual edits of the SQLite database:

1. The first change that must be made is that the Arcade script value stored in the style `meta` table key `dictionary_script` must be updated to reflect input parameters being passed in using `$feature` and `$config`. Previously, field values were passed in as `$fieldname` and a configuration values were passed in as `$configurationentryname`. In `3.0.0` dictionaries, all feature values are passed in via a `$feature` feature object and configuration values are passed in with a `$config` dictionary. So, the above example would be passed in now as `$feature.fieldname` and `$config.configurationentryname`. For the mil2525d dictionary, an example of this change would be `$sidc` changing to `$feature.sidc` and `$frame` changing to `$config.frame`.

1. The final change is that the value of key `dictionary_version` in the `meta` table must be changed from `2.0.0` to `3.0.0`.

Once the above steps are complete and your SQLite database is saved, your dictionary is upgraded.