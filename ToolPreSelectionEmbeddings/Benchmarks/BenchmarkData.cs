namespace ToolPreSelectionEmbeddings.Benchmarks;

public static class BenchmarkData
{
    public static readonly (string Query, string ExpectedTool)[] Queries =
    [
        ("I need to analyze this weather report: 'Temperature readings: 23.5°C at 9 AM, 27.8°C at noon, and 25.2°C at 3 PM'. Can you extract all the temperature numbers for processing?", "ExtractNumbers"),
        ("Our monitoring system detected these temperature patterns: 'ALERT-TEMP-HIGH' appears multiple times in today's log. How many occurrences are there? Don't worry about case sensitivity.", "CountOccurrences"),
        ("We need to generate a unique identifier for this weather event. It should include the prefix 'STORM-' and include the current timestamp for tracking purposes.", "GenerateId"),
        ("Can you check if this weather station ID 'WS-2024-ABC' matches our standard pattern '^WS-\\d{4}-[A-Z]{3}$'?", "ValidatePattern"),
        ("I have two timestamps from our Tokyo station: '2024-03-15T08:30:00Z' and '2024-03-15T15:45:00Z'. What's the time difference in hours?", "GetTimeDifference"),
        ("Our sensor recorded this reading at 2024-03-15T16:30:00Z. Is this within our business hours for data validation?", "IsBusinessHours"),
        ("Validate this credit card number from our weather subscription service: '4532-7153-3790-1241'", "ValidateCreditCard"),
        ("Can you validate this ISBN from our meteorology textbook: '0-7475-3269-9'?", "ValidateISBN"),
        ("Extract the main keywords from this weather alert: 'Severe thunderstorm warning with potential flash flooding and strong winds in the northeastern region.'", "ExtractKeywords"),
        ("What language is this weather alert in: 'Attention: Alerte météo severe pour la région parisienne'?", "DetectLanguage"),
        ("I need to securely store this weather data: 'According to the forecast for Paris today, it will be sunny with a high of 25°C and low of 15°C'. Can you hash it for me?", "HashString"),
        ("Our IoT sensor in Tokyo reports a temperature of 22.5°C, but our American clients need it in Fahrenheit for their dashboard. Can you convert it and make sure to handle decimal places correctly?", "ConvertLength"),
        ("I received this complex JSON response from our weather API and need it properly formatted for documentation: '{\"location\":{\"city\":\"Paris\",\"country\":\"FR\"},\"current\":{\"temp\":22.5,\"humidity\":65,\"wind\":12.3}}'", "FormatJson"),
        ("We need a secure password for our weather monitoring system. Could you generate one that incorporates today's date somehow?", "GeneratePassword"),
        ("Our system received this JSON with a contact number: '{\"station\":\"Paris-North\",\"contact\":{\"name\":\"Jean\",\"phone\":\"+33-555-0123\",\"backup\":\"+33-555-0124\"}}'. Can you validate the primary phone number?", "ValidatePhone"),
        ("We received a 2000-word weather report detailing atmospheric conditions over the Pacific for the next 72 hours. Can you create a concise summary focusing on the key weather patterns?", "Summarize"),
        ("The weather balloon recorded an altitude of 5.7843 meters. Convert this to feet for our American colleagues, and make sure to format it with exactly two decimal places for consistency.", "ConvertLength"),
        ("We have multiple weather-related TODO comments scattered across our src directory. Can you help me find all instances where we need to implement temperature conversion or rainfall calculations?", "SearchContent"),
        ("Our weather station recorded a significant temperature change at timestamp 2024-03-15T14:30:00Z. Can you format this timestamp in a more readable way for our report?", "FormatDate"),
        ("We have a weather report containing multiple URLs to satellite imagery. Can you validate each URL in this document to ensure all our image sources are secure and accessible?", "ValidateUrl"),
        ("I need to analyze this complex weather report: 'Temperature readings: 23.5°C at 9 AM, 27.8°C at noon, and 25.2°C at 3 PM, with wind speeds of 15.7 km/h and 18.2 km/h'. Can you extract all the numerical values for our database?", "ExtractNumbers"),
        ("Our monitoring system detected these critical patterns: 'ALERT-TEMP-HIGH' appears multiple times in today's log, and we need to know exactly how many for our incident report. Don't worry about case sensitivity.", "CountOccurrences"),
        ("We need to generate a unique identifier for this severe weather event. It should include the prefix 'STORM-' and include the current timestamp for our tracking system and historical records.", "GenerateId"),
        ("Can you verify if this weather station ID 'WS-2024-ABC' matches our standardized pattern '^WS-\\d{4}-[A-Z]{3}$'? This is critical for our data validation pipeline.", "ValidatePattern"),
        ("I have two precise timestamps from our Tokyo station: '2024-03-15T08:30:00Z' and '2024-03-15T15:45:00Z'. What's the exact time difference in hours for our data correlation analysis?", "GetTimeDifference"),
        ("Our automated sensor recorded this critical reading at 2024-03-15T16:30:00Z. We need to verify if this falls within our business hours for immediate data validation and alert processing.", "IsBusinessHours"),
        ("Please validate this credit card number from our premium weather subscription service: '4532-7153-3790-1241'. We need to ensure it's valid before processing the renewal.", "ValidateCreditCard"),
        ("Can you validate this ISBN from our new meteorology textbook: '0-7475-3269-9'? We need to verify it before adding it to our weather station's reference library.", "ValidateISBN"),
        ("Extract the main keywords from this urgent weather alert: 'Severe thunderstorm warning with potential flash flooding and strong winds exceeding 100km/h in the northeastern region, with possible tornado formation.'", "ExtractKeywords"),
        ("What language is this international weather alert in: 'Attention: Alerte météo severe pour la région parisienne avec des vents violents et des risques d'inondation'? We need to route it to the correct international response team.", "DetectLanguage"),
        ("I have a text file containing multiple ISBN numbers from our library catalog. Can you validate this one first: '0-7475-3269-9' before we process the whole file?", "ValidateISBN"),
        ("Our payment processor logged this credit card: '4532-7153-3790-1241' at timestamp 2024-03-15T16:30:00Z. First validate the card, and also check if this was during business hours.", "ValidateCreditCard"),
        ("This JSON response contains multiple phone numbers: '{\"contacts\":[{\"phone\":\"+1-555-0123\"},{\"phone\":\"(555) 0124\"}]}'. Can you validate the first phone number?", "ValidatePhone"),
        ("I need to extract all numerical values from this sales report: 'Total revenue: $12,345.67, Units sold: 89, Average price: $138.71, Growth: 23.5%'", "ExtractNumbers"),
        ("Our system generated this ID: 'TRX-20240315-abc123'. Can you verify if it matches our pattern '^TRX-\\d{8}-[a-z0-9]{6}$'?", "ValidatePattern"),
        ("This text appears in our error logs: 'ERROR-500' multiple times. Count how many occurrences there are, ignoring case sensitivity.", "CountOccurrences"),
        ("Generate a unique identifier for this transaction with prefix 'INV-' and include the current timestamp.", "GenerateId"),
        ("I have this technical documentation in French: 'Configuration du système et paramètres avancés'. What language is it in?", "DetectLanguage"),
        ("Here's a 500-word technical specification document. Can you extract the main keywords, focusing on technical terms?", "ExtractKeywords"),
        ("Format this JSON response from our payment API: '{\"transaction\":{\"id\":\"TX123\",\"amount\":99.99,\"status\":\"completed\"}}'", "FormatJson"),
        ("Our New York office made a commit at 15:30:00 UTC, and Tokyo approved it at 23:45:00 UTC. What's the time difference in hours?", "GetTimeDifference"),
        ("The system logged these numbers: '123.45, -67.89, 0.123, 456'. Can you extract only the positive numbers?", "ExtractNumbers"),
        ("This log file contains multiple email addresses. Can you validate this one first: 'support.team@company.co.uk'?", "ValidateEmail"),
        ("We received this data: '{\"temp\":22.5,\"pressure\":1013.2}'. Format it nicely with proper JSON indentation.", "FormatJson"),
        ("Calculate the sum of these values: 15.7 * 3.2 + 27.9, then format the result with 2 decimal places", "Calculate"),
        ("Our authentication service needs a secure password. Generate one that's suitable for database access.", "GeneratePassword"),
        ("I have timestamps from three different time zones. First, tell me if 2024-03-15T10:30:00Z is during business hours.", "IsBusinessHours"),
        ("Search through the config files for any URLs containing 'api.example.com' and validate each one.", "SearchContent"),
        ("This technical manual is 5000 words. Create a concise summary focusing on the main technical specifications.", "Summarize"),
        ("We got this error message: 'エラーが発生しました'. Can you detect what language it's in before we translate it?", "DetectLanguage"),
        ("I have an image file with extension '.jpg'. What's its MIME type for HTTP headers?", "GetMimeType"),
        ("Can you verify if these image dimensions are within our limits? Width: 1920px, Height: 1080px", "ValidateImageDimensions"),
        ("I need to calculate the aspect ratio for a video that's 3840 pixels wide and 2160 pixels high.", "CalculateAspectRatio"),
        ("If I invest $10,000 at 5% annual interest for 3 years with monthly compounding, how much will I have?", "CalculateCompoundInterest"),
        ("Calculate my monthly payment for a $300,000 mortgage at 3.5% interest rate for 30 years.", "CalculateLoanPayment"),
        ("I need to display this amount in euros: 1234567.89", "FormatCurrency"),
        ("Format this transaction amount of 50000 JPY in the proper Japanese currency format.", "FormatCurrency"),
        ("What's the distance between these coordinates: Tokyo (35.6762° N, 139.6503° E) and New York (40.7128° N, 74.0060° W)?", "CalculateDistance"),
        ("Can you verify if these coordinates are valid: Latitude 91.5, Longitude 180.2?", "ValidateCoordinates"),
        ("What's the cardinal direction when traveling from Paris (48.8566° N, 2.3522° E) to Rome (41.9028° N, 12.4964° E)?", "GetCardinalDirection")
    ];
}