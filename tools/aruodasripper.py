from bs4 import BeautifulSoup
from sys import argv
import requests

API_KEY = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJiMmM5ZWNiMi1lZGE2LTRiMGYtOTIzNi1lZjA1ODNmMTFiYzgiLCJyb2xlIjoiMiIsIm5iZiI6MTU1NjczMDY3NiwiZXhwIjoxNTU3MzM1NDc2LCJpYXQiOjE1NTY3MDE4NzYsImlzcyI6ImZsYXRyZW50LmNvbSIsImF1ZCI6ImZsYXRyZW50In0.WY7q5N3KGyR0--fc6CCLC_lROosQ1YE57CNvt3VAYuo'
API_CREATE_FLAT = 'https://localhost:5001/api/flat'
API_PUT_IMAGE_FORMAT = 'https://localhost:5001/api/image/{}'

def download(url, file_name):
    with open(file_name, "wb") as file:
        response = requests.get(url)
        file.write(response.content)

url = argv[1]

if (url == None):
    exit(0)

page = requests.get(url)
soup = BeautifulSoup(page.text, 'html.parser')

imageWrapper = soup.find('div', class_='thumb-row-wrapper')
details = soup.find('dl', class_='obj-details')
lhs = details.find_all('dt')
rhs = details.find_all('dd')

reqObject = {}

def findValue(lhs, rhs, search):
    item = list(filter(lambda x: search in x.get_text(), lhs))
    if len(item) == 0:
        return None
    else:
        index = lhs.index(item[0])
        return rhs[index].get_text().replace('m²', '').replace('€', '').strip()

def findAndSet(lhs, rhs, search, id, defaultVal):
    global reqObject
    item = findValue(lhs, rhs, search)
    if item is None:
        item = defaultVal
    reqObject[id] = item

findAndSet(lhs, rhs, 'Namo numeris', 'houseNumber', '11')
findAndSet(lhs, rhs, 'Buto numeris', 'flatNumber', '4')
findAndSet(lhs, rhs, 'Plotas', 'area', '40')
findAndSet(lhs, rhs, 'Kaina', 'price', '400')
findAndSet(lhs, rhs, 'Kambarių sk', 'roomCount', '4')
findAndSet(lhs, rhs, 'Aukštas', 'floor', '5')
findAndSet(lhs, rhs, 'Aukštų sk', 'totalFloors', '5')
findAndSet(lhs, rhs, 'Metai', 'yearOfConstruction', '2000')


# reqObject['flatNumber'] = rhs[1].get_text().strip()
# reqObject['area'] = rhs[2].get_text().replace('m²', '').strip()
# reqObject['price'] = rhs[3].get_text().replace('€', '').strip()
# reqObject['roomCount'] = rhs[4].get_text().strip()
# reqObject['floor'] = rhs[5].get_text().strip()
# reqObject['totalFloors'] = rhs[6].get_text().strip()
# reqObject['yearOfConstruction'] = rhs[7].get_text().strip()
# reqObject['buildingType'] = rhs[8].get_text().strip()
# reqObject['heating'] = rhs[9].get_text().strip()
furnished = findValue(lhs, rhs, 'Įrengimas')
reqObject['isFurnished'] = 'Įrengtas' in furnished
reqObject['price'] = reqObject['price'].replace(' ', '')
reqObject['area'] = reqObject['area'].replace(' ', '')



reqObject['features'] = list(map(lambda f: f.get_text().strip(), details.find_all('span', class_='special-comma')))
reqObject['minimumRentDays'] = 60
reqObject['tenantRequirements'] = 'Geras ir tvarkingas žmogus, pastovios pajamos, be gyvūnų. Pateikti pajamas patvirtinančius dokumentus.'
reqObject['isPublished'] = True

longText = soup.find('div', id='collapsedText').get_text().strip()
reqObject['description'] = longText


title = soup.find('h1', class_='obj-header-text').get_text().strip()

addressSplit = title.split(',')

reqObject['name'] = title
reqObject['country'] = 'Lietuva'
reqObject['postCode'] = '00000'
reqObject['houseNumber'] = '11'
reqObject['city'] = addressSplit[0]
reqObject['street'] = addressSplit[2]

images = imageWrapper.find_all('a')
array = []
for image in images:
    # if image.
    # pass
    imageUrl = image.attrs['href']
    if ('aruodas-img' not in imageUrl):
        continue
    array.append(imageUrl)
    # splitUrl = imageUrl.split('/')
    # fileName = splitUrl.pop()
    # objectName = splitUrl.pop()
    # download(imageUrl, '{}-{}'.format(objectName, fileName))

idx = 0
def getIdx():
    global idx
    idx = idx + 1
    return idx

reqObject['images'] = list(map(lambda i: dict(name='{}-{}'.format(getIdx(), i.split('/').pop())), array))

apiAuth = {}
apiAuth['Authorization'] = 'Bearer {}'.format(API_KEY)
# apiAuth['Content-Type'] = 'application/json'

import pprint
import json
createResponse = requests.post(API_CREATE_FLAT, json=reqObject, headers=apiAuth, verify=False)
pprint.pprint(createResponse.json())

for (idx, imageId) in enumerate(createResponse.json()['images'].keys()):
    singleImageUrl = array[idx]
    singleImage = requests.get(singleImageUrl)
    payload = {'image': (array[idx].split('/').pop(), singleImage.content, 'image/{}'.format(array[idx].split('.').pop()))}
    putResult = requests.put(API_PUT_IMAGE_FORMAT.format(imageId), headers=apiAuth, files=payload, verify=False)
    print(putResult.status_code)

exit(0)