# URL Shortener app
Simple Url Shortener app created using asp.net core

## How to start?
1. Open solution with Visual Studio
2. Set web application project as main project
3. Run the application

## How to use?
1. To shorten your URL go to "Add Short Url" tab
2. Enter your address (for example https://facebook.com)
3. Click "Create Short URL"
4. Find your short URL on the list
5. To use short URL go to: {siteurl}/{shorturl} (for example: http://localhost:5167/abcdef )

## Key assumptions
I had 2 main assumptions while implementing:
- To make each short url unique
- To be able to easily test code

I think I managed to achieve both. Urls are random 6 characters strings, which gives us a lot of possible combinations and just in case we can allways extend length of short url. It was a bit challanging to test this code because outcome of generating short url is random. That's why I decided to split logic in tiny services - so I can mock my random values.

## Future Ideas
There are many fields to improve in this app.
- Add some nice exception handling - so when short url doesn't exists user is redirected to nice looking 404 page
- Add page after creating which shows you only newly created url insted of whole list
- Regular user shouldn't have access to list of all short urls - it should be part of some admin panel



