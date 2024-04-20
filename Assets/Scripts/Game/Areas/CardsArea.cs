using System;
using System.Collections;
using UnityEngine;

public class CardsArea : ACardsArea<Card>
{
    public Coroutine TurnRandom(float delay) => TraversingRandom(delay, Turn);
    public Coroutine TurnRepeat(float delay) => TraversingRepeat(delay, Turn);

    private IEnumerator Turn(Card card) => card.Turn_Coroutine();

    public void CreateCards(int size, Action<Card> action)
    {
        int countNew = size * size;
        if (_cardsActive.Count == countNew)
            return;

        Card card;
        while (_cardsActive.Count > countNew)
        {
            card = _cardsActive.Pop();
            card.Deactivate(_repository);
            _cardsRepository.Push(card);

        }
        while (_cardsActive.Count < countNew)
        {
            if (_cardsRepository.Count > 0)
            {
                card = _cardsRepository.Pop();
                card.Activate(_thisTransform);
            }
            else
            {
                card = Instantiate(_prefabCard, _thisTransform);
                card.EventSelected += action;
            }

            _cardsActive.Push(card);
        }

        _cardsActive.Size = _sizeArea = size;
        _cardsActive.CopyToShuffledArray(_cardsRandom);
    }
}
