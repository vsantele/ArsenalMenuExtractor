# Arsenal Menu Extractor

![Build](https://github.com/vsantele/ArsenalMenuExtractor/actions/workflows/main.yml/badge.svg) ![GitHub Repo stars](https://img.shields.io/github/stars/vsantele/ArsenalMenuExtractor?style=social) [![wakatime](https://wakatime.com/badge/user/50b62530-8c31-4f47-94c5-590cb1842ae5/project/3bf6a87c-3343-444c-9303-73386e776284.svg)](https://wakatime.com/badge/user/50b62530-8c31-4f47-94c5-590cb1842ae5/project/3bf6a87c-3343-444c-9303-73386e776284)

This is a tool to extract the Arsenal menu from the [Unamur](https://www.unamur.be/services/vecu/arsenal-restaurants-salles/menu-tarif) website.

## Description

This project runs on Azure Functions. Extracted data is stored in Cosmos DB and can be exported to a ICS file to display menu in a calendar.

The menu is a image, so we need to extract the text from it. To do so, we use the [Form Recognizer API](https://learn.microsoft.com/en-us/azure/applied-ai-services/form-recognizer/overview?tabs=v3-0) from [Azure Cognitive Services](https://learn.microsoft.com/en-us/azure/cognitive-services/what-are-cognitive-services).

The extraction is done by the `MenuExtractor` function. It is triggered by a Timer Trigger. The timer is set to run every sunday at 6:00 PM. The function will then extract the menu for the current week and store it in Cosmos DB.

The `GetCalendar` function is triggered by an HTTP request. It will then return the menu in a ICS file. This file or URL can be added to a calendar to display the menu. If you use the URL, the calendar will be updated automatically.

## How to use

Simply add the URL of the `GetCalendar` function to your calendar. The URL is [https://arsenalextractor-2209-func.azurewebsites.net/api/getcalendar](https://arsenalextractor-2209-func.azurewebsites.net/api/getcalendar)

If you want to select the menu for the summary, you can add the `?menu={yourMenu}` query parameter to the URL. Valid options are:

- `vege` : Vegetarian menu
- `day`: Day menu
- `chef`: Chef menu

Example: [https://arsenalextractor-2209-func.azurewebsites.net/api/getcalendar?menu=vege](https://arsenalextractor-2209-func.azurewebsites.net/api/getcalendar?menu=vege) will show the vegetarian menu in the summary of the event.

You can see the menu directly on your browser with [this link](https://calendar.google.com/calendar/u/0/embed?src=risbi89uhhld64l3do7d9nbgpmst9lsk@import.calendar.google.com&ctz=Europe/Brussels).

## How to deploy

To deploy this project, you can use the [Bicep](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep) file inside `infrastructure` folder. Then you can upload the code to Azure Functions using the [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) for Visual Studio Code.

The bicep file will create the following resources:

- Azure Functions
- Azure Cognitive Services Form Recognizer
- Azure Cosmos DB
- Azure Storage Account
- Azure Application Insights

It will also set the following application settings:

- `CosmosDbConnectionString`: Connection string to the Cosmos DB
- `AzureFormRecognizer:Endpoint`: Endpoint of the Form Recognizer API
- `AzureFormRecognizer:ApiKey`: API key of the Form Recognizer API

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change. I will be happy to review your PR.

## Support me

My website: [vsantele.dev](https://vsantele.dev)

[![Twitter Follow](https://img.shields.io/twitter/follow/vsantele?style=social)](https://twitter.com/vsantele)

[![GitHub followers](https://img.shields.io/github/followers/vsantele?style=social)](https://github.com/vsantele)

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vsantele)

ETH: 0x4E7fFf04Eb923F6d83Beab3F87b57a78286CefA3
