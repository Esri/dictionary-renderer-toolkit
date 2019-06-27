# Tips for creating custom dictionaries

* The dictionary configuration and script can be edited in any text editor. To extract, open the .stylx in a SQLite database browser. In the `meta` table, double-click the value field for the `dictionary_configuration` or `dictionary_script`, select all and copy. Paste into a text editor, such as Notepad++. Once edits have been made, you can copy the changes back into the style. If you are saving it as stand-alone file, save it as a JSON file.

* When editing the Arcade script, you can check the syntax and ensure the expected keys are returned using https://developers.arcgis.com/arcade/playground/. In the Expression window, add variables for the properties in the configuration. Below the variables, copy the modified script and click Test. If there are no errors, the results return a string of the values. Variables for the dictionaries included in ArcGIS Pro are available [here](../variable_declarations)

* Because dictionary styles can be very large, it can be easier to create or edit symbols in a separate, smaller style and then merge them into the dictionary style once editing is complete. Ensure that the symbols have the correct key once they are merged into the dictionary. Use the [Merge Style add-in](../Add-Ins/Merge_Styles) with the Replace Keys option to merge the symbols into the dictionary style while preserving the keys.

* When customizing an existing dictionary, such as the military symbology dictionaries, the easiest way to create a new symbol is to copy an existing symbol and modify the properties accordingly. This approach preserves common properties, ensuring consistency.
