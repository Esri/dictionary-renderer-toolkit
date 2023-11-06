# Tips for creating custom dictionaries

* The dictionary configuration and script can be edited in any text editor. To extract, open the .stylx in a SQLite database browser. In the `meta` table, double-click the value field for the `dictionary_configuration` or `dictionary_script`, select all and copy. Paste into a text editor, such as Notepad++. Once edits have been made, you can copy the changes back into the style. If you are saving it as stand-alone file, save it as a JSON file.

* When editing the Arcade script, you can check the syntax and ensure the expected keys are returned using https://developers.arcgis.com/arcade/playground/. In the Expression window, add variables for the properties in the configuration. Below the variables, copy the modified script and click Test. If there are no errors, the results return a string of the values. Variables for the dictionaries included in ArcGIS Pro are available [here](../variable_declarations)

* Because dictionary styles can be very large, it can be easier to create or edit symbols in a separate, smaller style and then merge them into the dictionary style once editing is complete. Ensure that the symbols have the correct key once they are merged into the dictionary. Use the [Merge Style add-in](../Add-Ins/Merge_Styles) with the Replace Keys option to merge the symbols into the dictionary style while preserving the keys.

* When customizing an existing dictionary, such as the military symbology dictionaries, the easiest way to create a new symbol is to copy an existing symbol and modify the properties accordingly. This approach preserves common properties, ensuring consistency.

* Reserved characters, such as semi-colon (`;`), pipe (`|`), and period (`.`), should not be used in the style item keys for symbols in the dictionary. These types of special characters can cause problems when trying to parse the dictionary script as well as when trying to build proper URLs for a dictionary as a web style.

* When using invalid symbols in the dictionary to symbolize features whose attributes do not produce an existing key in the style, it is necessary to hard code specific keys for the invalid symbols. For point symbols the key needs to be `Invalid_P`. For line symbols the key needs to be `Invalid_L`. For polygon symbols the key needs to be `Invalid_A`.

* Names used for symbol attributes, text attributes and configuration fields should be unique.  The name should not be duplicated even if they are in different groups.

* When using Arcade operators that require numeric, date or time data types ensure the dictionary version is 4.0.0.  See [Working with different field types](../docs/working-with-different-field-types.md) for examples.
