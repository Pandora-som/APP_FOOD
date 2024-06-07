from telebot import types
import telebot
import mysql.connector
from mysql.connector import connect, Error
from os import getenv
from dotenv import load_dotenv

load_dotenv('data.env')  # Загрузка .env файла

print(getenv('HOST'),
      getenv('USER'),
      getenv('DATABASE'),
      getenv('PASSWORD'))  # Прроверка считывания .env

diet_bot = telebot.TeleBot('7084126348:AAEvTraP1V0dktqmDg-h9VSphWqx3P9fY4M')
diet = {}
questions = ['❔Вопрос 1 из 12:\n'
             'Можно ли вам молочную продукцию?',
             '❔Вопрос 2 из 12:\n'
             'Можно ли вам жирные виды мяса или рыбы?',
             '❔Вопрос 3 из 12:\n'
             'Можно ли вам сладкое?',
             '❔Вопрос 4 из 12:\n'
             'Можно ли вам острое?',
             '❔Вопрос 5 из 12:\n'
             'Можно ли вам бобовые культуры?',
             '❔Вопрос 6 из 12:\n'
             'Можно ли вам овощи?',
             '❔Вопрос 7 из 12:\n'
             'Можно ли вам хлебобулочные изделия (выпечка)?',
             '❔Вопрос 8 из 12:\n'
             'Можно ли вам грибы?',
             '❔Вопрос 9 из 12:\n'
             'Можно ли вам кофе?',
             '❔Вопрос 10 из 12:\n'
             'Можно ли вам копчёности/колбасные изделия?',
             '❔Вопрос 11 из 12:\n'
             'Можно ли вам маринованную продукцию?',
             '❔Вопрос 12 из 12:\n'
             'Можно ли вам соусы?'
             ]

tables = {'1': [True, False, False, False, True, True, True, True, False, True, True, True],
          '2': [False, False, False, True, False, True, False, True, True, False, True, True],
          '3': [False, False, True, True, True, True, True, True, True, True, False, True],
          '4': [False, False, True, True, False, False, False, True, False, True, False, True],
          '5': [True, False, True, True, True, False, False, True, False, True, True, True],
          '6': [True, False, False, False, False, True, True, False, True, True, True, True],
          '7': [True, False, False, False, False, True, False, False, True, False, True, True],
          '8': [False, False, False, True, True, True, True, True, True, False, False, False],
          '9': [True, False, False, True, False, True, False, False, True, True, False, True],
          '10': [True, False, False, True, False, True, False, False, True, True, False, True],
          '11': [True, False, False, False, True, True, True, True, False, False, True, False],
          '12': [True, False, True, False, True, True, False, True, True, False, False, False],
          '13': [True, False, True, False, False, False, True, False, True, False, True, True],
          '14': [False, False, False, True, True, False, True, True, True, False, False, True],
          '15': [True, False, True, False, True, True, True, True, False, True, True, False]}


@diet_bot.message_handler(commands=['start', 'help'])
def login(message):
    info = f'Привет, {message.from_user.first_name}. Наш бот создан для того, чтобы помочь вам сформировать меню, подходящее под ваши предпочтения. Поэтому мы предлагаем пройти опрос, на основе которого мы подберём соответствующие блюда.'
    markup = types.InlineKeyboardMarkup()
    markup.add(types.InlineKeyboardButton(text='Начать', callback_data='login'))

    diet_bot.send_message(message.chat.id, info, reply_markup=markup)


@diet_bot.callback_query_handler(func=lambda callback: True)
def login_proc(callback):  # Функция для записи пользователя в бд
    global mess_id
    if callback.data == 'login':
        try:
            # Запрос пользователя из бд
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                user_avail = f'''SELECT * FROM users WHERE User_ID = {callback.from_user.id}'''
                with connection.cursor() as cursor:
                    cursor.execute(user_avail)
                    avail = cursor.fetchall()
            # Если пользователя нет, то записывается в варианты ответов его номер и его данные в таблицу пользователей
            if avail == []:
                sql_login = "INSERT INTO users (User_ID, Firstname, Surname, Table_number_id, Question_number, Answer_variant_id) VALUES (%s, %s, %s, %s, %s, %s)"
                sql_answs = "INSERT INTO answer_variant (User_answer_ID) VALUE (%s)"  # %s - параметрирование
                # Добавление в бд ответов пользователя
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        password=getenv('PASSWORD')
                ) as connection:
                    with connection.cursor() as cursor:
                        cursor.execute(sql_answs, (
                        callback.from_user.id,))  # Передача значений параметров по порядку их упоминания
                        connection.commit()
                # Добавление в бд пользователя
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        password=getenv('PASSWORD')
                ) as connection:
                    with connection.cursor() as cursor:
                        cursor.execute(sql_login, (
                        callback.from_user.id, callback.from_user.first_name, callback.from_user.last_name, 16, 0,
                        callback.from_user.id))
                        connection.commit()

                markup_menu = types.InlineKeyboardMarkup()
                markup_menu.add(types.InlineKeyboardButton(text='Посмотреть меню🍽️', callback_data='menu'))

                markup_poll = types.ReplyKeyboardMarkup(resize_keyboard=True)
                markup_poll.add(types.KeyboardButton(text='Пройти тест✅'))

                diet_bot.send_message(callback.message.chat.id, 'Если вы уже проходили тест:', reply_markup=markup_menu)
                diet_bot.send_message(callback.message.chat.id,
                                      'Или можете пройти его, нажав на кнопку под клавиатурой️👇',
                                      reply_markup=markup_poll)
            # Если пользователь уже есть, то ему сразу выводятся сообщения с функционалом
            else:
                markup_menu = types.InlineKeyboardMarkup()
                markup_menu.add(types.InlineKeyboardButton(text='Посмотреть меню🍽️', callback_data='menu'))

                markup_poll = types.ReplyKeyboardMarkup(resize_keyboard=True)
                markup_poll.add(types.KeyboardButton(text='Пройти тест✅'))

                diet_bot.send_message(callback.message.chat.id, 'Если вы уже проходили тест:', reply_markup=markup_menu)
                diet_bot.send_message(callback.message.chat.id,
                                      'Или можете пройти его, нажав на кнопку под клавиатурой️👇',
                                      reply_markup=markup_poll)

        except mysql.connector.errors.IntegrityError as e:
            print(e)
    # Если пользователь выбрал меню
    if callback.data == 'menu':
        menu_list = types.InlineKeyboardMarkup(row_width=3)
        breakfast = types.InlineKeyboardButton(text='Завтрак🍳', callback_data='breakfast')
        lunch = types.InlineKeyboardButton(text='Обед🍜', callback_data='lunch')
        dinner = types.InlineKeyboardButton(text='Ужин🍝', callback_data='dinner')
        menu_list.row(breakfast, lunch, dinner)

        mess = diet_bot.send_message(callback.message.chat.id, "Выберите приём пищи, чтобы посмотреть блюда",
                                     reply_markup=menu_list)
        diet[callback.from_user.id] = {'mess_id': mess.message_id}

    # Если пользователь выбрал завтрак
    if callback.data == 'breakfast':
        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            table = f'''SELECT `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10`,
                        `11`, `12` FROM answer_variant WHERE User_answer_ID = {callback.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(table)
                answs = cursor.fetchall()[0]
                print(answs)

        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            table = f'''SELECT * FROM tables'''
            with connection.cursor() as cursor:
                cursor.execute(table)
                tables = cursor.fetchall()
                tables_w_numb = {}
                for table in tables:
                    tables_w_numb[table[0]] = table[1::]
                print(tables_w_numb)
        if answs in tables_w_numb.values():
            table_numb = next(
                key for key, value in tables_w_numb.items() if value == answs)  # Получение ключа из словаря

            # Выбор блюд, у которых тип завтрак и которые есть в столе пользователя
            dishes_menu = '🍳Ваши блюда на завтрак:\n'
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                select_dishes_query = f'''SELECT dishes.Name FROM dishes 
                    JOIN menu_dishes ON menu_dishes.Dishes_id = dishes.DISHES_ID 
                    JOIN meal_types ON dishes.Meal_type = meal_types.id
                    WHERE menu_dishes.Tables_id = {table_numb} AND meal_types.id = 1'''
                with connection.cursor() as cursor:
                    cursor.execute(select_dishes_query)
                    result = cursor.fetchall()
                    for i in result:
                        dishes_menu += '🍴' + i[0] + '\n'

            back_markup = types.InlineKeyboardMarkup()
            back_markup.add(types.InlineKeyboardButton(text='🔙Назад к меню', callback_data='back'))
            # Изменение сообщения бота на общий список блюд стола
            diet_bot.edit_message_text(chat_id=callback.message.chat.id,
                                       message_id=diet[callback.from_user.id]['mess_id'], text=dishes_menu,
                                       reply_markup=back_markup)

        else:
            dishes_menu = '🍳Ваши блюда на завтрак:\n'
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                select_dishes_query = f'''SELECT Name FROM dishes 
                    JOIN meal_types ON dishes.Meal_type = meal_types.id 
                    WHERE meal_types.id = 1'''
                with connection.cursor() as cursor:
                    cursor.execute(select_dishes_query)
                    result = cursor.fetchall()
                    for i in result:
                        dishes_menu += '🍴' + i[0] + '\n'

            back_markup = types.InlineKeyboardMarkup()
            back_markup.add(types.InlineKeyboardButton(text='🔙Назад к меню', callback_data='back'))
            diet_bot.edit_message_text(chat_id=callback.message.chat.id,
                                       message_id=diet[callback.from_user.id]['mess_id'], text=dishes_menu,
                                       reply_markup=back_markup)

    if callback.data == 'lunch':
        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            table = f'''SELECT `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10`,
                        `11`, `12` FROM answer_variant WHERE User_answer_ID = {callback.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(table)
                answs = cursor.fetchall()[0]
                print(answs)

        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            table = f'''SELECT * FROM tables'''
            with connection.cursor() as cursor:
                cursor.execute(table)
                tables = cursor.fetchall()
                tables_w_numb = {}
                for table in tables:
                    tables_w_numb[table[0]] = table[1::]
                print(tables_w_numb)
        if answs in tables_w_numb.values():
            table_numb = next(
                key for key, value in tables_w_numb.items() if value == answs)  # Получение ключа из словаря

            dishes_menu = '🍜Ваши блюда на обед:\n'
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                select_dishes_query = f'''SELECT dishes.Name FROM dishes 
                    JOIN menu_dishes ON menu_dishes.Dishes_id = dishes.DISHES_ID 
                    JOIN meal_types ON dishes.Meal_type = meal_types.id
                    WHERE menu_dishes.Tables_id = {table_numb} AND meal_types.id = 2'''
                with connection.cursor() as cursor:
                    cursor.execute(select_dishes_query)
                    result = cursor.fetchall()
                    for i in result:
                        dishes_menu += '🍴' + i[0] + '\n'

            back_markup = types.InlineKeyboardMarkup()
            back_markup.add(types.InlineKeyboardButton(text='🔙Назад к меню', callback_data='back'))
            diet_bot.edit_message_text(chat_id=callback.message.chat.id,
                                       message_id=diet[callback.from_user.id]['mess_id'], text=dishes_menu,
                                       reply_markup=back_markup)

        else:
            dishes_menu = '🍜Ваши блюда на обед:\n'
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                select_dishes_query = f'''SELECT Name FROM dishes 
                    JOIN meal_types ON dishes.Meal_type = meal_types.id 
                    WHERE meal_types.id = 2'''
                with connection.cursor() as cursor:
                    cursor.execute(select_dishes_query)
                    result = cursor.fetchall()
                    for i in result:
                        dishes_menu += '🍴' + i[0] + '\n'

            back_markup = types.InlineKeyboardMarkup()
            back_markup.add(types.InlineKeyboardButton(text='🔙Назад к меню', callback_data='back'))
            diet_bot.edit_message_text(chat_id=callback.message.chat.id,
                                       message_id=diet[callback.from_user.id]['mess_id'], text=dishes_menu,
                                       reply_markup=back_markup)

    if callback.data == 'dinner':
        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            table = f'''SELECT `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10`,
                        `11`, `12` FROM answer_variant WHERE User_answer_ID = {callback.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(table)
                answs = cursor.fetchall()[0]
                print(answs)

        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            table = f'''SELECT * FROM tables'''
            with connection.cursor() as cursor:
                cursor.execute(table)
                tables = cursor.fetchall()
                tables_w_numb = {}
                for table in tables:
                    tables_w_numb[table[0]] = table[1::]
                print(tables_w_numb)
        if answs in tables_w_numb.values():
            table_numb = next(
                key for key, value in tables_w_numb.items() if value == answs)  # Получение ключа из словаря

            dishes_menu = '🍝Ваши блюда на ужин:\n'
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                select_dishes_query = f'''SELECT dishes.Name FROM dishes 
                    JOIN menu_dishes ON menu_dishes.Dishes_id = dishes.DISHES_ID 
                    JOIN meal_types ON dishes.Meal_type = meal_types.id
                    WHERE menu_dishes.Tables_id = {table_numb} AND meal_types.id = 3'''
                with connection.cursor() as cursor:
                    cursor.execute(select_dishes_query)
                    result = cursor.fetchall()
                    for i in result:
                        dishes_menu += '🍴' + i[0] + '\n'

            back_markup = types.InlineKeyboardMarkup()
            back_markup.add(types.InlineKeyboardButton(text='🔙Назад к меню', callback_data='back'))
            diet_bot.edit_message_text(chat_id=callback.message.chat.id,
                                       message_id=diet[callback.from_user.id]['mess_id'], text=dishes_menu,
                                       reply_markup=back_markup)

        else:
            dishes_menu = '🍝Ваши блюда на ужин:\n'
            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                select_dishes_query = f'''SELECT Name FROM dishes 
                    JOIN meal_types ON dishes.Meal_type = meal_types.id 
                    WHERE meal_types.id = 3'''
                with connection.cursor() as cursor:
                    cursor.execute(select_dishes_query)
                    result = cursor.fetchall()
                    for i in result:
                        dishes_menu += '🍴' + i[0] + '\n'

            back_markup = types.InlineKeyboardMarkup()
            back_markup.add(types.InlineKeyboardButton(text='🔙Назад к меню', callback_data='back'))
            diet_bot.edit_message_text(chat_id=callback.message.chat.id,
                                       message_id=diet[callback.from_user.id]['mess_id'], text=dishes_menu,
                                       reply_markup=back_markup)

    if callback.data == 'back':
        menu_list = types.InlineKeyboardMarkup(row_width=3)
        breakfast = types.InlineKeyboardButton(text='Завтрак🍳', callback_data='breakfast')
        lunch = types.InlineKeyboardButton(text='Обед🍜', callback_data='lunch')
        dinner = types.InlineKeyboardButton(text='Ужин🍝', callback_data='dinner')
        menu_list.row(breakfast, lunch, dinner)

        diet_bot.edit_message_text(chat_id=callback.message.chat.id, message_id=diet[callback.from_user.id]['mess_id'],
                                   text="Выберите приём пищи, чтобы посмотреть блюда", reply_markup=menu_list)


@diet_bot.message_handler(content_types=['text'])
def poll(message):
    global questions
    global diet
    global mess_id

    # Если пользователь новый
    if message.from_user.id not in diet:
        # Получение из бд номера вопроса, на котором он последний раз ответил
        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            question_sql = f'''SELECT Question_number FROM users WHERE User_ID = {message.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(question_sql)
                quest = cursor.fetchall()[0][0]

        diet[message.from_user.id] = {'step': quest}
        q = questions[diet[message.from_user.id]['step']]

        poll_markup = types.ReplyKeyboardMarkup(resize_keyboard=True, row_width=2)

        poll_markup.add(types.KeyboardButton('Да'))
        poll_markup.add(types.KeyboardButton('Нет'))

        diet_bot.send_message(message.chat.id, q, reply_markup=poll_markup)

        # Добавление к номеру вопроса 1 в бд
        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            question_sql = f'''UPDATE users SET Question_number = Question_number + 1 WHERE User_ID = {message.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(question_sql)
                connection.commit()

    # Если пользователь уже есть в системе
    elif message.from_user.id in diet:
        # Получение номера вопроса пользователя
        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            question_sql = f'''SELECT Question_number FROM users WHERE User_ID = {message.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(question_sql)
                diet[message.from_user.id]['step'] = cursor.fetchall()[0][0]

        with connect(
                host=getenv('HOST'),
                user=getenv('USER'),
                database=getenv('DATABASE'),
                port=getenv('PORT'),
                password=getenv('PASSWORD')
        ) as connection:
            question_sql = f'''UPDATE users SET Question_number = Question_number + 1 WHERE User_ID = {message.from_user.id}'''
            with connection.cursor() as cursor:
                cursor.execute(question_sql)
                connection.commit()

        # Пользователь ответил на все вопросы
        if diet[message.from_user.id]['step'] == len(questions):
            # Если пользователь ответил да, то в бд на номер вопроса ставиться 1, иначе - 0
            if message.text == 'Да':
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        port=getenv('PORT'),
                        password=getenv('PASSWORD')
                ) as connection:
                    answ = f'''UPDATE answer_variant SET `{diet[message.from_user.id]['step']}` = 1 WHERE User_answer_ID = {message.from_user.id}'''
                    with connection.cursor() as cursor:
                        cursor.execute(answ)
                        connection.commit()

            elif message.text == 'Нет':
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        port=getenv('PORT'),
                        password=getenv('PASSWORD')
                ) as connection:
                    answ = f'''UPDATE answer_variant SET `{diet[message.from_user.id]['step']}` = 0 WHERE User_answer_ID = {message.from_user.id}'''
                    with connection.cursor() as cursor:
                        cursor.execute(answ)
                        connection.commit()

            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                table = f'''SELECT `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10`,
                        `11`, `12` FROM answer_variant WHERE User_answer_ID = {message.from_user.id}'''
                with connection.cursor() as cursor:
                    cursor.execute(table)
                    answs = cursor.fetchall()[0]

            with connect(
                    host=getenv('HOST'),
                    user=getenv('USER'),
                    database=getenv('DATABASE'),
                    port=getenv('PORT'),
                    password=getenv('PASSWORD')
            ) as connection:
                table = f'''SELECT * FROM tables'''
                with connection.cursor() as cursor:
                    cursor.execute(table)
                    tables = cursor.fetchall()
                    tables_w_numb = {}
                    for table in tables:
                        tables_w_numb[table[0]] = table[1::]

            # Если ответы, которые дал пользователь совпадают со столами, то ему выводится этот стол, если не совпадает, то выводятся все блюда
            if answs not in tables_w_numb.values():
                # Обнуление номера вопроса пользователя
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        port=getenv('PORT'),
                        password=getenv('PASSWORD')
                ) as connection:
                    question_sql = f'''UPDATE users SET Question_number = 0 WHERE User_ID = {message.from_user.id}'''
                    with connection.cursor() as cursor:
                        cursor.execute(question_sql)
                        connection.commit()

                try:
                    menu_list = types.InlineKeyboardMarkup(row_width=3)
                    breakfast = types.InlineKeyboardButton(text='Завтрак🍳', callback_data='breakfast')
                    lunch = types.InlineKeyboardButton(text='Обед🍜', callback_data='lunch')
                    dinner = types.InlineKeyboardButton(text='Ужин🍝', callback_data='dinner')
                    menu_list.row(breakfast, lunch, dinner)

                    mess = diet_bot.send_message(message.chat.id, "Выберите приём пищи, чтобы посмотреть блюда",
                                                 reply_markup=menu_list)
                    diet[message.from_user.id] = {'mess_id': mess.message_id}

                    markup = types.ReplyKeyboardMarkup(resize_keyboard=True)
                    markup.add(types.InlineKeyboardButton(text='Пройти тест снова✅'))
                    diet_bot.send_message(message.chat.id,
                                          'Если хотите пройти тест ещё раз, нажмите кнопку под клавиатурой️👇',
                                          reply_markup=markup)

                except Error as e:
                    print(e)

            else:
                try:
                    menu_list = types.InlineKeyboardMarkup(row_width=3)
                    breakfast = types.InlineKeyboardButton(text='Завтрак🍳', callback_data='breakfast')
                    lunch = types.InlineKeyboardButton(text='Обед🍜', callback_data='lunch')
                    dinner = types.InlineKeyboardButton(text='Ужин🍝', callback_data='dinner')
                    menu_list.row(breakfast, lunch, dinner)

                    mess = diet_bot.send_message(message.chat.id, "Выберите приём пищи, чтобы посмотреть блюда",
                                                 reply_markup=menu_list)
                    diet[message.from_user.id] = {'mess_id': mess.message_id}

                    markup = types.ReplyKeyboardMarkup(resize_keyboard=True)
                    markup.add(types.InlineKeyboardButton(text='Пройти тест снова✅'))
                    diet_bot.send_message(message.chat.id,
                                          'Если хотите пройти тест ещё раз, нажмите кнопку под клавиатурой️👇',
                                          reply_markup=markup)

                except Error as e:
                    print(e)
        # Если пользователь ещё не ответил на все вопросы (процесс опроса)
        else:
            q = questions[diet[message.from_user.id]['step']]

            poll_markup = types.ReplyKeyboardMarkup(resize_keyboard=True, row_width=2)

            poll_markup.add(types.KeyboardButton('Да'))
            poll_markup.add(types.KeyboardButton('Нет'))

            if message.text == 'Да':
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        port=getenv('PORT'),
                        password=getenv('PASSWORD')
                ) as connection:
                    answ = f'''UPDATE answer_variant SET `{diet[message.from_user.id]['step']}` = 1 WHERE User_answer_ID = {message.from_user.id}'''
                    with connection.cursor() as cursor:
                        cursor.execute(answ)
                        connection.commit()

            elif message.text == 'Нет':
                # diet[message.from_user.id]['answs'].append(False)
                with connect(
                        host=getenv('HOST'),
                        user=getenv('USER'),
                        database=getenv('DATABASE'),
                        port=getenv('PORT'),
                        password=getenv('PASSWORD')
                ) as connection:
                    answ = f'''UPDATE answer_variant SET `{diet[message.from_user.id]['step']}` = 0 WHERE User_answer_ID = {message.from_user.id}'''
                    with connection.cursor() as cursor:
                        cursor.execute(answ)
                        connection.commit()

            diet_bot.send_message(message.chat.id, q, reply_markup=poll_markup)


diet_bot.infinity_polling()
