import os
import sys
from langchain.document_loaders import TextLoader, DirectoryLoader
from langchain.indexes import VectorstoreIndexCreator
from langchain.llms import OpenAI
from langchain.chat_models import ChatOpenAI



def ChatGPTquery(query):
	os.environ["OPENAI_API_KEY"] = "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"
	folder_path = os.path.join(os.path.dirname(__file__), 'model_files')
	#loader = TextLoader("text.txt")
	loader = DirectoryLoader(folder_path, glob="*.txt")
	index = VectorstoreIndexCreator().from_loaders([loader])
	return index.query(query)


def ReturnString(string):
	return string+" was entered"

	