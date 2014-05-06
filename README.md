Spellcheck
==========

A quick implementation of a Burkhard-Keller tree. This is a tree in which each edge has a weight equal to the Levenshtein distance (the number of changes that must be made to convert one string into another). This lets us quickly traverse the tree for things spelled similarly to some search string, letting us determine whether a word is spelled correctly or offering spelling suggestions.
