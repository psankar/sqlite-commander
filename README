Sqlite-Commander
================

Sqlite-Commander is a project that is aimed to display sqlite databases in Terminal using ncurses. Mono.NCurses is used as the CUI framework. The name is inspired from Midnight-Commander written by Miguel De Icaza.

All the source code is available to you in LGPL v2.1 only.

Drop me a mail if you like the software.


Features
========
+ Shows the list of tables in the left pane. 
+ Allows you to navigate the list of tables using arrow keys.
+ Shows the first 30 records for the selected table in the right pane
+ Non-printable columns are shown with a ???
+ Maximum of 30 characters are displayed per column. Column data exceeding this length are shown with "..."


ToDo
====
There are few things which will be good to have. But I don't see a pressing need for these features in my workflow. So I have not implemented these. But patches are more than welcome. If you feel very interested about some of these items, send me a mail and I may do it :) Sending mails with patches for these features will be even more awesome.

+ Add a scroll bar and show more than 30 columns. Should be moderate/easy to implement.
+ Add a dialog box to execute any random SQL statement. Should be fairly trivial to implement.
+ Create Events such that pressing Enter on the records list will pass the currently selected record to a custom script/command. 
+ Copy selections of records to clipboard. Should be moderate/easy to implement.


Hacking Sqlite-Commander
=========================
1) Install mono-devel packages, make
2) Checkout MonoCurses package from Mono source code repositories: git clone https://github.com/mono/mono-curses.git
3) ./configure --prefix=/usr ; sudo make install
4) Checkout Sqlite-Commander sources from github: git clone https://github.com/curiosity/sqlite-commander.git
5) Substitute "<SQLITE FILE PATH>" in the Makefile with your sqlite file path.
6) make run

You can also execute sqlite-commander manually and pass the database file path as a command line option.
