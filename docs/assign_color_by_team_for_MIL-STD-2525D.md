# Assign color by team for MIL-STD-2525D

## Assign color based on team designation

This example shows how to assign the color of a framed symbol based on the team designation.  It creates a custom dictionary that is a modified version of the MIL-STD-2525D dictionary with an updated script that defines colors based on the team designation.  The teams are defined in the data and connected to the dictionary using the new team symbol field property. 

## Create a custom dictionary

To modify the dictionary for MIL-STD-2525D you must first create a custom dictionary.
1. Download the [Joint Military Symbology MIL-STD-2525D](https://www.arcgis.com/home/item.html?id=44b781991d194dd8bc423e642c1932c5) file from ArcGIS Online.
2. Open the copy of the mil2525d.stylx in an SQLite database editor.
3. In the `meta` table, update the `dictionary_name` to the name of the custom dictionary.
4. Make the style file editable by changing the `readonly` value to `false`.

## Update configuration file

The dictionary configuration file controls the symbology and text fields that are shown in the Dictionary Symbology pane. This file also controls the options that are shown in the configuration section of the Symbology pane. To modify the dictionary configuration, copy it from the meta table in the .stylx file and paste the dictionary configuration file in Notepad++. See [Tips for creating custom dictionaries](tips-for-creating-custom-dictionaries.md).

_Add new symbol field:_ In the configuration file add `team` to the list of symbol fields. This adds `team` in the Symbology Fields section of the Dictionary Symbology pane. Setting this field to the attribute field that contains the team information in the data will allow the dictionary to determine the team for each feature.

  `"symbol": ["sidc", "affiliation", "extendedfunctioncode", "status", "hqtffd", "echelonmobility", "civilian", "direction", "team"],`
  
## Update the script

To change the color of framed symbols based on the team designation it is necessary to add an additional if/else block to the script in the frame symbol section that deals with the frame fill color. To modify the script, copy it from the meta table in the .stylx file and open the dictionary script file in Notepad++. See [Tips for creating custom dictionaries](tips-for-creating-custom-dictionaries.md).

The additional if/else block will check if the team symbol field is empty, if it is not then it will assign the color based on the team's name.  If the field is empty, then the standard color logic will be applied.  The decode function assigns a color for each team name as well as assigns a default color.  If the string in the team field does not match the team names in the list it will draw with the default color.  If it is empty it will fall back to the standard color logic.

```
    // frame fill color
    var _fill_color;
    if (!isempty($feature.team))
        _fill_color = decode($feature.team, 'Alpha', '#83C7DC', 'Bravo', '#00A8DC', 'Charlie', '#006B8C', 'Delta', '#F67585', 'Echo', '#FF3031', 'Foxtrot', '#C80000', '#C0C0C0');
    else if ($feature.civilian == '1' && _affiliation != '6')
        _fill_color = decode($config.colors, 'DARK', '#500050', 'MEDIUM', '#800080', '#FFA1FF'); // civilian
    else
        _fill_color = decode(_affiliation,
                '3', decode($config.colors, 'DARK', '#006B8C', 'MEDIUM', '#00A8DC', '#80E0FF'), // friendly
                '4', decode($config.colors, 'DARK', '#00A000', 'MEDIUM', '#00E200', '#AAFFAA'), // neutral
                '6', decode($config.colors, 'DARK', '#C80000', 'MEDIUM', '#FF3031', '#FF8080'), // hostile
                decode($config.colors, 'DARK', '#E1DC00', 'MEDIUM', '#FFFF00', '#FFFF80')); // unknown

```

Information on Arcade functions is available at https://developers.arcgis.com/arcade/function-reference/

## Verify syntax
Once the script has been updated it is good practice to verify the syntax of the script before adding it back into the dictionary. The syntax of the script can be verified online using the Arcade playground.
1. Go to https://developers.arcgis.com/arcade/playground/
2. Copy the text from [mil2525d_app6d_arcade_vars.json](../variable_declarations/mil2525d_app6d_arcade_vars.json) into the expression window. These are values that you can change to match the symbols you are adding. The values will be used for the string that is returned in the results.
3. Add new team option below other variables: `team: "Alpha",`
4. Copy in the text from the edited dictionary script. Paste below the text from `mil2525d_app6d_arcade_vars`.
5. Click Test. If there is a syntax error, the line with the error is reported in the results. If there are no errors the results show a string of the values returned. You can change the values at the top to test what different keys are returned.
6. To add the script back into the dictionary, copy and paste the script into `dictionary_script` in the `meta` table of the .stylx.

