SOURCES = Autobahn.cs

Autobahn.exe: $(SOURCES) Makefile 
	gmcs -debug $(SOURCES) -pkg:mono-curses

run: Autobahn.exe
	mono --debug Autobahn.exe; stty sane
