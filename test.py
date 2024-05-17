import os
import re
import json
import shutil

def fetch_reviews():
    # Directory where your JSON files are located
    directory = "reviews/"
    project_root = os.path.dirname(os.path.abspath(__file__))  # Get the project root directory

    # Regular expression to match the file names
    file_pattern = re.compile(r'reviews\((\w+)\)_(\w+)\.json')

    # Function to read JSON data from a file
    def read_json_file(file_path):
        with open(file_path, 'r') as json_file:
            data = json.load(json_file)
        return data

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
        output_filename = os.path.join(project_root, f"{original_asin}.json")

        # Convert the new JSON object to a JSON string and save it to the output file
        with open(output_filename, 'w') as output_file:
            json.dump(new_json, output_file, ensure_ascii=False)

        print(f"Saved {output_filename}")

def remove_directory(dir_path):
    try:
        shutil.rmtree(dir_path)
        print(f"Removed {dir_path}")
    except OSError as e:
        print(f"Error: {e}")

# Rest of the code remains the same

if __name__ == "__main__":
    fetch_reviews()

