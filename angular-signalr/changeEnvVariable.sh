#!/bin/bash

# Replace placeholders in environment.ts with actual environment variable values
if [ -z "$API_URL" ]; then
  echo "API_URL environment variable is not set. Using default value."
  export API_URL="localhost"
else
  echo "Using API_URL from environment variable: $API_URL"
  export API_URL=$API_URL
fi

# Run envsubst to replace placeholders in environment.ts with actual environment variable values
envsubst < src/environments/environment.ts > src/environments/environment.tmp.ts && \
mv src/environments/environment.tmp.ts src/environments/environment.ts
