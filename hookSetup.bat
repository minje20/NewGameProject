set file_name=hook-origin.txt
set path=.git/hooks/

copy /Y %file_name% "%path%pre-commit"
copy /Y %file_name% "%path%pre-push"

#@pause