import json
import os
import re
import shutil
import subprocess
import threading
import sys
import openai

def get_reviews(asin):
    # Function to execute the command for a single ASIN
    def execute_amazon_buddy(asin):
        command = f"mkdir reviews && npx amazon-buddy reviews {asin} --random-ua --filetype json"
        result = subprocess.run(command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
        if result.returncode == 0:
            print(f"Command for ASIN {asin} executed successfully:\n")
        else:
            print(f"Error executing command for ASIN {asin}:\n{result.stderr}")

    # Create a thread for each ASIN
    execute_amazon_buddy(asin)
    print("All commands executed.")

    # After all commands are executed, move the JSON files to the "reviews" folder
    mv_command = "for /r %i in (reviews.json) do move \"%i\" reviews\\"

    result = subprocess.run(mv_command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
    if result.returncode == 0:
        print("JSON files moved to 'reviews' folder successfully.")
    else:
        print(f"Error moving JSON files to 'reviews' folder:\n{result.stderr}")


def fetch_reviews():

    # Directory where your JSON files are located
    directory = "reviews/"
    output_directory = "feeding_data/"  # Output directory

    # Regular expression to match the file names
    file_pattern = re.compile(r'reviews\((\w+)\)_(\w+)\.json')

    # Function to read JSON data from a file
    def read_json_file(file_path):
        with open(file_path, 'r', encoding='utf-8') as json_file:
            data = json.load(json_file)
        return data

    # Ensure the output directory exists
    os.makedirs(output_directory, exist_ok=True)

    # List of JSON file paths matching the naming convention
    json_files = []

    # List files in the directory
    for filename in os.listdir(directory):
        # Match the file name with the pattern
        match = file_pattern.match(filename)
        if match:
            json_file_path = os.path.join(directory, filename)
            json_files.append(json_file_path)

    # Iterate through each JSON file
    for json_file_path in json_files:
        # Read JSON data from the file
        data = read_json_file(json_file_path)

        # Extract the "original" values from "asin"
        original_asins = list(set([item["asin"]["original"] for item in data]))

        # Concatenate all reviews into a single string
        all_reviews = "\n".join([item["review"] for item in data])

        # Create a new JSON object with "original" and "reviews" fields
        new_json = {"original_asins": original_asins, "all_reviews": all_reviews}

        # Generate the output filename using the original ASIN
        original_asin = original_asins[0]  # Assuming there's only one original ASIN
        output_filename = os.path.join(output_directory, f"{original_asin}.json")

        # Convert the new JSON object to a JSON string and save it to the output file
        with open(output_filename, 'w', encoding='utf-8') as output_file:
            json.dump(new_json, output_file, ensure_ascii=False)

        print(f"Saved {output_filename}")

def remove_directory(dir_path):
    try:
        shutil.rmtree(dir_path)
        print(f"Removed {dir_path}")
    except OSError as e:
        print(f"Error: {e}")

def gen_AI_magik(asin):
    api_key = "your_api_key"

    # Initialize the OpenAI API client
    openai.api_key = api_key

    # Replace the next line with the actual path to your data file
    data_file_path = 'feeding_data/' + asin + '.json'

    # Load the data from the data file
    with open(data_file_path, 'r', encoding='utf-8') as f:
        data = json.load(f)
        review_data = data['all_reviews']

    # Define prompts for the three tasks
    description_prompt = "Imagine you are a salesperson and you are given reviews of a product. Your task is to write a positive product description with the help of positive reviews under 70 words. REVIEWS:" + review_data
    use_cases_prompt = "Imagine you are a salesperson and you are given reviews of a product. Your task is to give the top 3 use cases of the product. REVIEWS:" + review_data
    phrases_prompt = "Imagine you are a salesperson and you are given reviews of a product. Your task is to give the top 5 positive phrases to describe the product. REVIEWS:" + review_data

    # Define a function to make an API request and save the response to a file
    def make_api_request_and_save(prompt, filename):
        response = openai.Completion.create(
            engine="text-davinci-002",
            prompt=prompt,
            max_tokens=150
        )
        with open(filename, 'w') as file:
            file.write(response.choices[0].text)

    # Create a dictionary to store the filenames for each task
    filenames = {
        "description": f"{asin}_description.txt",
        "usecases": f"{asin}_usecases.txt",
        "phrases": f"{asin}_phrases.txt"
    }

    # Make API requests one by one and save the responses
    make_api_request_and_save(description_prompt, filenames["description"])
    make_api_request_and_save(use_cases_prompt, filenames["usecases"])
    make_api_request_and_save(phrases_prompt, filenames["phrases"])

    # Create a dictionary with the response data
    response_data = {
        "asin_number": asin,
        "description": open(filenames["description"]).read(),
        "Use_cases": open(filenames["usecases"]).read(),
        "phrases": open(filenames["phrases"]).read()
    }

    # Save the response_data as a JSON file
    response_filename = f"{asin}_response.json"
    if os.path.exists(data_file_path):
        with open(response_filename, 'w', encoding='utf-8') as json_file:
            json.dump(response_data, json_file, indent=4)
    else:
        print(f"File not found: {data_file_path}")

    # Delete the text files
    for filename in filenames.values():
        os.remove(filename)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        exit(0)

    asin = sys.argv[1]

    get_reviews(asin)
    fetch_reviews()
    remove_directory('reviews')
    gen_AI_magik(asin)
