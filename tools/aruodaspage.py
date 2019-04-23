from bs4 import BeautifulSoup
from sys import argv
import subprocess
import requests

page = argv[1]


if (page == None):
    exit(0)

if ('https' in page):
    page = requests.get(page)
else:
    page = requests.get('https://www.aruodas.lt/butu-nuoma/puslapis/{}/'.format(page))

soup = BeautifulSoup(page.text, 'html.parser')

for row in soup.find_all('tr', class_='list-row'):
    flat = row.find('a')
    if flat is not None:
        print(flat.attrs['href'])
        url = 'https://www.aruodas.lt{}'.format(flat.attrs['href'])
        p = subprocess.Popen([ 'py', './aruodasripper.py', url ])
        p.wait()

