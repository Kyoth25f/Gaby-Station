interaction-LookAt-name = Encostar o olhar
interaction-LookAt-description = Encare o vazio e sinta ele encarar você de volta.
interaction-LookAt-success-self-popup = Você encara {THE($target)}.
interaction-LookAt-success-target-popup = Você sente {THE($user)} te encarando...
interaction-LookAt-success-others-popup = {THE($user)} encara {THE($target)}.

interaction-Hug-name = Abraçar
interaction-Hug-description = Um abraço por dia mantém os horrores psicológicos além da sua compreensão longe.
interaction-Hug-success-self-popup = Você abraça {THE($target)}.
interaction-Hug-success-target-popup = {THE($user)} te abraça.
interaction-Hug-success-others-popup = {THE($user)} abraça {THE($target)}.

interaction-KnockOn-name = Bater
interaction-KnockOn-description = Bata no alvo para chamar atenção.
interaction-KnockOn-success-self-popup = Você bate em {THE($target)}.
interaction-KnockOn-success-target-popup = {THE($user)} bate em você.
interaction-KnockOn-success-others-popup = {THE($user)} bate em {THE($target)}.

interaction-WaveAt-name = Acenar
interaction-WaveAt-description = Acene para o alvo. Se você estiver segurando um item, você vai acenar com ele.
interaction-WaveAt-success-self-popup = Você acena {$hasUsed ->
    [false] para {THE($target)}.
    *[true] com seu {$used} para {THE($target)}.
}
interaction-WaveAt-success-target-popup = {THE($user)} acena {$hasUsed ->
    [false] para você.
    *[true] com {POSS-PRONOUN($user)} {$used} para você.
}
interaction-WaveAt-success-others-popup = {THE($user)} acena {$hasUsed ->
    [false] para {THE($target)}.
    *[true] com {POSS-PRONOUN($user)} {$used} para {THE($target)}.
}
