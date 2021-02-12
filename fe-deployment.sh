#! /bin/bash

echo ">>>>>>>> deploying fe <<<<<<<<<<<"
echo

FEPATH="./FE"
PUBLICPATH="./public"

echo "changing directory to ${FEPATH}"
echo

command cd ${FEPATH}

#TODO: make this step optional with some flag
command npm install

command cd ".."
command cd ${PUBLICPATH}

command pwd

echo "cleaning ${pwd}"
echo

# eval "rm -rf !(*gitkeep) ${PUBLICPATH} "
# command rm -rf '!(*.gitkeep)'
# eval "rm -rf '!(*.gitkeep)'"
# rm -rf '!(*.gitkeep)'
rm *.html
rm -r assets

cd ..

pwd

cp FE/dist/*.html ./public/
mkdir public/assets
cp FE/dist/*js ./public/assets
