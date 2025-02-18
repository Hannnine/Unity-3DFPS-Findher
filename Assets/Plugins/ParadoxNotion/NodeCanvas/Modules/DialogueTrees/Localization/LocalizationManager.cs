// using UnityEngine.Localization.Settings;

// namespace GameManager.Localization {
// 	public static class LocalizationManager {
// 		// Static method to get a localized string
// 		public static string GetLocalizedString(string tableName, string key) {
// 			// current language
// 			string currentLanguage = LocalizationSettings.SelectedLocale.Identifier.Code;

// 			// get localized string
// 			var localizedString = GetLocalizedStringFromTable(tableName, key, currentLanguage);
// 			return localizedString;
// 		}

// 		// private method
// 		private static string GetLocalizedStringFromTable(string tableName, string key, string languageCode) {
// 			// table
// 			var table = LocalizationSettings.StringDatabase.GetTable(tableName);

// 			if (table != null) {
// 				var entry = table.GetEntry(key);

// 				if (entry != null) {
// 					return entry.GetLocalizedString(languageCode);
// 				}
// 				else {
// 					return $"Key '{key}' not found in the localization table '{tableName}'.";
// 				}
// 			}
// 			else {
// 				return $"Table '{tableName}' not found.";
// 			}
// 		}
// 	}
// }

