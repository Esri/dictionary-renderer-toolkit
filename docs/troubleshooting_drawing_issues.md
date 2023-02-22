# Troubleshooting drawing issues

If your custom dictionary is not drawing there may be an issue with the structure of the dictionary, the logic in the dictionary script, or the attributes in the data may not return a valid key. 

## Validating dictionaries
The dictionaries are analyzed for possible errors to help users troubleshoot issues for why the dictionary will not load or will not draw.  When the dictionary is first added it will be analyzed to ensure that the structure of the dictionary is valid and to look for syntax errors that prevent the correct execution of the script.  If an error is detected it is reported in a banner in the symbology pane for the dictionary renderer.  The message in the banner should give guidance on how to correct the error.  Examples of these types of errors include script parsing errors, missing dictionary components or discrepancies between the script and configuration.

The dictionary is also evaluated at draw time to identify arcade script errors that are encountered at runtime such as an undefined variable or a division by zero.  To view these errors it is necessary to use the Diagnostic Monitor in ArcGIS Pro.  The errors will be reported on the Log tab whenever they are encountered.  For information about the Diagnostic Monitor see the [help topic.](https://pro.arcgis.com/en/pro-app/latest/get-started/arcgis-diagnostic-monitor.htm) 

Viewing errors in the Diagnostic Monitor
1.	_Open the Diagnostic Monitor:_ The monitor must be open before the error is encountered to see it reported in the log.  There are two ways to launch the Diagnostic Monitor.

a.	While ArcGIS Pro is open, press Ctrl+Alt+M.

b.	Start ArcGIS Pro from the command line using the switch /enablediagnostics.

2.	_View the Log tab:_ In the Diagnostic Monitor, switch to the Log tab.  The Log tab is a live event log viewer where various diagnostic information is recorded.

3.	_Filter errors in the Log tab:_ Numerous events are continuously recorded in the log.  You can filter these events to find the dictionary errors more easily in a couple of ways.
a.	There are four types of events reported and these can be filtered using the check boxes at the top of the Log tab.  Unchecking the boxes for warning, information and debug will leave just the errors.
b.	You can also filter based on key words.  Filtering using the string ‘dictionary script’ will filter just the errors evaluating the dictionary script.

## Validating attributes
Another important step in troubleshooting potential drawing issues is to ensure that you have special symbols able to represent invalid or missing keys in your custom dictionary.  While the error checking above is useful for troubleshooting why the dictionary may not load or draw at all, having invalid symbols will allow you to troubleshoot individual features that are not drawing as expected.  In the custom dictionary, you can author any symbol to represent an invalid symbol whose attributes do not produce an existing key in the style.  However, it is necessary to hard code specific keys for the invalid symbols.  For point symbols the key needs to be `Invalid_P`. For line symbols the key needs to be `Invalid_L`. For polygon symbols the key needs to be `Invalid_A`.
