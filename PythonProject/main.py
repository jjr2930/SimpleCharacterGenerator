import requests
import cv2
import Config
import os
import argparse

'''
gender : male, female, neutral
blendshapesValues = length 20, floating first 10 : -500~500, next 10 : -200~200
'''
def SendToGameEngine(gender:str, shapes:list, texturePath:str, outputDirectory:str):
    datas = {
        "gender":"",
        "shapes":shapes,
        "texturePath":texturePath,
        "outputDir":outputDirectory
    }

    url = f"{Config.IP}{Config.PORT}"
    
    response = requests.post(url,data=datas)

    return "good" == response.text

def Inference(prompt : str):
    print("Start inference")
    return (None, None, None)


def Train(prompt : str, outputDirectory : str):
    (gender, shapes,texture ) = Inference(prompt)
    cv2.imwrite(Config.TEXTURE_TEMP_PATH, texture)
    SendToGameEngine(gender,shapes,outputDirectory)

if __name__ == "__main__":
    argumentParser = argparse.ArgumentParser()
    argumentParser.add_argument("--prompt")
    argumentParser.add_argument("--output")
    args = argumentParser.parse_args()
    
    Train(args.prompt, args.output)
