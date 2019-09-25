using System;
using System.Collections.Generic;

namespace ExerciseEngine.Language
{
    public interface IWordDictionary
    {
        /// <summary>
        /// Gets the random word from dictionary.
        /// </summary>
        /// <returns>The random word.</returns>
        /// <param name="length">Length.</param>
        void GetRandomWord(Action<string> callback, uint length);

        /// <summary>
        /// Checks if the word exists in the dictionary.
        /// </summary>
        /// <returns><c>true</c>, if word was checked, <c>false</c> otherwise.</returns>
        /// <param name="word">Word.</param>
        void CheckWord(Action<bool> callback, string word);

        /// <summary>
        /// Gets random words not containing specific letters and having unique characters.
        /// </summary>
        /// <returns>The words not containing letters.</returns>
        /// <param name="notContainingLetters">Set of letters that cannot occur in word.</param>
        /// <param name="minLength">Minimum word length.</param>
        /// <param name="maxLength">Maximum word length.</param>
        /// <param name="maxAmount">Maximum amount of words to find.</param>
        void GetWordsNotContainingLetters(Action<List<string>> callback, List<char> notContainingLetters, uint minLength, uint maxLength, uint maxAmount);

        /// <summary>
        /// Gets the random word with unique letters.
        /// </summary>
        /// <returns>The random word with unique letters.</returns>
        /// <param name="length">Length.</param>
        void GetRandomWordWithUniqueLetters(Action<string> callback, uint length);

        /// <summary>
        /// Gets random similar words to a word made from letterSet, that are differnt from original word by letterDifference amount having unique characters.
        /// </summary>
        /// <returns>The similar words.</returns>
        /// <param name="letterSet">Letter set.</param>
        /// <param name="letterDifference">Amount of different letters from original word.</param>
        /// <param name="minLength">Minimum word length.</param>
        /// <param name="maxLength">Maximum word length.</param>
        /// <param name="maxAmount">Maximum amount of words to find.</param>
        void GetSimilarWords(Action<List<string>> callback, char[] letterSet, uint letterDifference, uint minLength, uint maxLength, uint maxAmount);

		/*List<string> GetWords(string firstLettersCombination, string lastLettersCombination, int maxLetters = -1, int minLetters = -1);
		string GetRandomWordWithCombinatorialDepth(int minDepth, int maxDepth, int maxLetters = -1);
		List<string> GetAllWordsFromLetters(string sourceWord, bool filterByNumber = false);
		string GetFirstLastTupleWithNumWords(int minNumWords, int maxNumWords, int maxLetters = -1, int minLetters = -1);
		List<string> GetWordsContainingLetters(List<string> containingLetters, List<string> notContainingLetters, int minLength, int maxLength, int maxAmount);*/
    }
}
